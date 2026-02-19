using Escola.Domain.Entities;
using Escola.Services.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola.Domain.Contracts
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);

        Task<int> CreateAsync(UserCreateRecord input, CancellationToken ct = default);

        Task ChangePasswordAsync(UserChangePasswordRecord input, CancellationToken ct = default);

        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
