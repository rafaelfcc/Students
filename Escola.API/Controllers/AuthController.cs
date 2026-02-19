using Escola.API.DTO;
using Escola.API.JwtAux;
using Escola.Domain.Contracts;
using Escola.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _users;
        private readonly JwtService _jwt;
        private readonly IPasswordHasher<User> _hasher;

        public AuthController(IUserRepository users, JwtService jwt, IPasswordHasher<User> hasher)
        {
            _users = users;
            _jwt = jwt;
            _hasher = hasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request, CancellationToken ct)
        {
            if (request is null) return BadRequest();

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username e Password são obrigatórios.");

            var user = await _users.GetByUsernameAsync(request.Username, ct);
            if (user is null) return Unauthorized("Credenciais inválidas.");

            var result = _hasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Credenciais inválidas.");

            var token = _jwt.GenerateToken(user.Id.ToString(), user.Username);
            return Ok(new { token });
        }
    }
}
