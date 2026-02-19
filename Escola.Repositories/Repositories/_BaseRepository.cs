using Escola.Data.Context;
using Escola.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Escola.Repositories.Repositories
{
    public abstract class _BaseRepository<T> : _IBaseRepository<T> where T : class
    {
        protected readonly DataContext _context;
        protected readonly DbSet<T> _dbSet;

        protected _BaseRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(new object?[] { id }, ct);
        }

        public virtual async Task<List<T>> GetListAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken ct = default)
        {
            return await _dbSet.AsNoTracking().Where(filter).ToListAsync(ct);
        }

        public virtual async Task<TId> InsertAsync<TId>(T item, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(item, ct);
            await _context.SaveChangesAsync(ct);

            var idProp = typeof(T).GetProperty("Id");

            if (idProp is not null)
            {
                var value = idProp.GetValue(item);
                if (value is not null)
                    return (TId)value;
            }

            return default!;
        }

        public virtual async Task<bool> UpdateAsync(T item, CancellationToken ct = default)
        {
            _dbSet.Update(item);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public virtual async Task<bool> DeleteAsync<TId>(TId id, CancellationToken ct = default)
        {
            var existing = await GetByIdAsync(id, ct);
            if (existing is null) return false;

            _dbSet.Remove(existing);
            return await _context.SaveChangesAsync(ct) > 0;
        }
    }
}
