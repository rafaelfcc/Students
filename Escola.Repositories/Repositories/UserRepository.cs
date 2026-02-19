using Escola.Data.Context;
using Escola.Domain.Contracts;
using Escola.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Escola.Repositories.Repositories
{
    public class UserRepository : _BaseRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { }

        public Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
            => base.GetByIdAsync(id, ct);

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == username, ct);
        }

        public async Task<int> InsertAsync(User user, CancellationToken ct = default)
        {
            return await base.InsertAsync<int>(user, ct);
        }

        public Task<bool> UpdateAsync(User user, CancellationToken ct = default)
            => base.UpdateAsync(user, ct);

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
            => base.DeleteAsync(id, ct);
    }
}
