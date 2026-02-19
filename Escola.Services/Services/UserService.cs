using Escola.Domain.Contracts;
using Escola.Domain.Entities;
using Escola.Services.Records;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola.Services.Services
{
    public class UserService : _BaseService<User>, IUserService
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher<User> _hasher;

        public UserService(IUserRepository users, IPasswordHasher<User> hasher) : base(users)
        {
            _users = users;
            _hasher = hasher;
        }

        public Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
            => _users.GetByIdAsync(id, ct);

        public Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username é obrigatório.");

            return _users.GetByUsernameAsync(username.Trim(), ct);
        }

        public async Task<int> CreateAsync(UserCreateRecord input, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(input.UserName))
                throw new ArgumentException("Username é obrigatório.");

            if (string.IsNullOrWhiteSpace(input.Password))
                throw new ArgumentException("Password é obrigatório.");

            var username = input.UserName.Trim();

            var existing = await _users.GetByUsernameAsync(username, ct);
            if (existing is not null)
                throw new InvalidOperationException("Username já existe.");

            var user = new User { Username = username };
            user.Password = _hasher.HashPassword(user, input.Password);

            var id = await _users.InsertAsync(user, ct);
            return id;
        }

        public async Task ChangePasswordAsync(UserChangePasswordRecord input, CancellationToken ct = default)
        {
            if (input.UserId <= 0)
                throw new ArgumentException("Id inválido.");

            if (string.IsNullOrWhiteSpace(input.PrevPassword))
                throw new ArgumentException("PrevPassword é obrigatório.");

            if (string.IsNullOrWhiteSpace(input.NewPassword))
                throw new ArgumentException("NewPassword é obrigatório.");

            var existing = await _users.GetByIdAsync(input.UserId, ct);

            if (existing is null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            var verify = _hasher.VerifyHashedPassword(existing, existing.Password, input.PrevPassword);

            if (verify == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Senha atual inválida.");

            existing.Password = _hasher.HashPassword(existing, input.NewPassword);

            var ok = await _users.UpdateAsync(existing, ct);

            if (!ok)
                throw new KeyNotFoundException("Usuário não encontrado.");
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0) throw new ArgumentException("Id inválido.");
            return _users.DeleteAsync(id, ct);
        }
    }
}
