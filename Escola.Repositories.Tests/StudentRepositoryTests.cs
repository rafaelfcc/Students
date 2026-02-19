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
    public class StudentRepositoryTests
    {
        [Fact]
        public async Task InsertAsync_ShouldInsert_AndGetById()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new StudentRepository(ctx);

            var student = new Student
            {
                Nome = "Kiko Loureiro",
                Idade = 37,
                Serie = 2,
                NotaMedia = 8.5,
                Endereco = "Rua A, 123",
                NomePai = "Pai Kiko",
                NomeMae = "Mae Kiko",
                DataNascimento = new DateTime(1988, 6, 16)
            };

            var id = await repo.InsertAsync(student);

            Assert.True(id > 0);

            var byId = await repo.GetByIdAsync(id);
            Assert.NotNull(byId);
            Assert.Equal("Kiko Loureiro", byId!.Nome);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFound_ShouldReturnNull()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new StudentRepository(ctx);

            var result = await repo.GetByIdAsync(999999);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingStudent()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new StudentRepository(ctx);

            var id = await repo.InsertAsync(new Student
            {
                Nome = "Rafael Bittencourt",
                Idade = 40,
                Serie = 3,
                NotaMedia = 9.1,
                Endereco = "Rua B, 456",
                NomePai = "Pai Rafael",
                NomeMae = "Mae Rafael",
                DataNascimento = new DateTime(1985, 10, 22)
            });

            var loaded = await repo.GetByIdAsync(id);
            Assert.NotNull(loaded);

            loaded!.Endereco = "Rua B, 456 - AP 12";
            loaded.NotaMedia = 9.7;

            var updated = await repo.UpdateAsync(loaded);
            Assert.True(updated);

            var check = await repo.GetByIdAsync(id);
            Assert.NotNull(check);
            Assert.Equal("Rua B, 456 - AP 12", check!.Endereco);
            Assert.Equal(9.7, check.NotaMedia);
        }

        [Fact]
        public async Task UpdateAsync_WhenNotFound_ShouldThrowDbUpdateConcurrencyException()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new StudentRepository(ctx);

            var ghost = new Student
            {
                Id = 777777,
                Nome = "Ghost Student",
                Idade = 1,
                Serie = 1,
                NotaMedia = 0,
                Endereco = "Nowhere",
                NomePai = "None",
                NomeMae = "None",
                DataNascimento = DateTime.UtcNow.Date
            };

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repo.UpdateAsync(ghost));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrueWhenDeleted_AndFalseWhenNotFound()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new StudentRepository(ctx);

            var id = await repo.InsertAsync(new Student
            {
                Nome = "Luiz Mariutti",
                Idade = 34,
                Serie = 1,
                NotaMedia = 7.2,
                Endereco = "Rua C, 789",
                NomePai = "Pai Luiz",
                NomeMae = "Mae Luiz",
                DataNascimento = new DateTime(1991, 1, 1)
            });

            var deleted = await repo.DeleteAsync(id);
            Assert.True(deleted);

            var deletedAgain = await repo.DeleteAsync(id);
            Assert.False(deletedAgain);

            var deletedMissing = await repo.DeleteAsync(999999);
            Assert.False(deletedMissing);
        }
    }
}
