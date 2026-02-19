using Escola.Data.Context;
using Escola.Domain.Entities;
using Escola.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola.Repositories.Tests
{
    internal static class InMemoryDbFactory
    {
        public static DataContext CreateContext(string? dbName = null)
        {
            dbName ??= $"EscolaTests_{Guid.NewGuid():N}";

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(dbName)
                .EnableSensitiveDataLogging()
                .Options;

            return new DataContext(options);
        }
    }
}
