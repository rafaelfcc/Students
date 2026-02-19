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
    public class UserRepositoryTests
    {
        [Fact]
        public async Task InsertAsync_ShouldInsert_AndGetById_AndGetByUsername()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new UserRepository(ctx);

            var user = new User
            {
                Username = "BruceDickinson",
                Password = "pwd-1"
            };

            var id = await repo.InsertAsync(user);

            Assert.True(id > 0);

            var byId = await repo.GetByIdAsync(id);
            Assert.NotNull(byId);
            Assert.Equal("BruceDickinson", byId!.Username);

            var byUsername = await repo.GetByUsernameAsync("BruceDickinson");
            Assert.NotNull(byUsername);
            Assert.Equal(id, byUsername!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFound_ShouldReturnNull()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new UserRepository(ctx);

            var result = await repo.GetByIdAsync(999999);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUsernameAsync_WhenNotFound_ShouldReturnNull()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new UserRepository(ctx);

            var result = await repo.GetByUsernameAsync("NaoExiste");
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingUser()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new UserRepository(ctx);

            var id = await repo.InsertAsync(new User { Username = "SteveHarris", Password = "pwd-1" });

            var loaded = await repo.GetByIdAsync(id);
            Assert.NotNull(loaded);

            loaded!.Password = "pwd-UPDATED";

            var updated = await repo.UpdateAsync(loaded);
            Assert.True(updated);

            var check = await repo.GetByIdAsync(id);
            Assert.NotNull(check);
            Assert.Equal("pwd-UPDATED", check!.Password);
        }

        [Fact]
        public async Task UpdateAsync_WhenNotFound_ShouldThrowDbUpdateConcurrencyException()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new UserRepository(ctx);

            var ghost = new User
            {
                Id = 123456,
                Username = "GhostUser",
                Password = "pwd"
            };

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repo.UpdateAsync(ghost));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrueWhenDeleted_AndFalseWhenNotFound()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new UserRepository(ctx);

            var id = await repo.InsertAsync(new User { Username = "BruceDickinson", Password = "pwd" });

            var deleted = await repo.DeleteAsync(id);
            Assert.True(deleted);

            var deletedAgain = await repo.DeleteAsync(id);
            Assert.False(deletedAgain);

            var deletedMissing = await repo.DeleteAsync(999999);
            Assert.False(deletedMissing);
        }

        [Fact]
        public async Task InsertAsync_DuplicatedUsername_InMemory_DoesNotGuaranteeException()
        {
            await using var ctx = InMemoryDbFactory.CreateContext();
            var repo = new UserRepository(ctx);

            await repo.InsertAsync(new User { Username = "SteveHarris", Password = "pwd-1" });
            await repo.InsertAsync(new User { Username = "SteveHarris", Password = "pwd-2" });

            var found = await repo.GetByUsernameAsync("SteveHarris");
            Assert.NotNull(found);
            Assert.Equal("SteveHarris", found!.Username);
        }
    }
}
