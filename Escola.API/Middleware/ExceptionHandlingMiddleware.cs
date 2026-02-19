using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Escola.API.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await WriteProblemDetailsAsync(context, ex);
            }
        }

        private async Task WriteProblemDetailsAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception. TraceId: {TraceId}", context.TraceIdentifier);

            var (status, title) = ex switch
            {
                DbUpdateException => (StatusCodes.Status400BadRequest, "Database Update Error"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                InvalidOperationException => (StatusCodes.Status409Conflict, "Conflict"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Instance = context.Request.Path,
                Detail = (status == 500)
                    ? "Erro inesperado."
                    : ex.Message
            };

            if (ex is DbUpdateException dbEx)
            {
                if (dbEx.InnerException is SqlException sqlEx)
                {
                    if (sqlEx.Number is 2627 or 2601)
                    {
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        problem.Status = StatusCodes.Status409Conflict;
                        problem.Title = "Conflict";
                        problem.Detail = "Registro duplicado (violação de chave única).";
                    }
                    else if (sqlEx.Number == 547)
                    {
                        problem.Detail = "Referência inválida (violação de chave estrangeira).";
                    }
                    else if (sqlEx.Number == 515)
                    {
                        problem.Detail = "Campo obrigatório não informado (valor nulo em coluna obrigatória).";
                    }

                    if (_env.IsDevelopment())
                        problem.Extensions["sql"] = sqlEx.Message;
                }
                else
                {
                    if (_env.IsDevelopment())
                        problem.Extensions["db"] = dbEx.InnerException?.Message ?? dbEx.Message;
                }
            }

            problem.Extensions["traceId"] = context.TraceIdentifier;

            if (_env.IsDevelopment())
            {
                problem.Extensions["exception"] = ex.GetType().FullName;
                problem.Extensions["stackTrace"] = ex.StackTrace;
            }

            context.Response.Clear();
            context.Response.StatusCode = status;
            context.Response.ContentType = "application/problem+json";

            var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
