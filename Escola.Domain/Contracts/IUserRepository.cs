using Escola.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Escola.Domain.Contracts
{
    public interface IUserRepository : _IBaseRepository<User>
    {
        Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task<int> InsertAsync(User user, CancellationToken ct = default);
        Task<bool> UpdateAsync(User user, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
