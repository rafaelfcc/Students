using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Escola.Domain.Contracts
{
    public interface _IBaseRepository<T>
    {
        Task<T?> GetByIdAsync<TId>(TId id, CancellationToken ct = default);

        Task<List<T>> GetListAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken ct = default);

        Task<TId> InsertAsync<TId>(T item, CancellationToken ct = default);
        Task<bool> UpdateAsync(T item, CancellationToken ct = default);
        Task<bool> DeleteAsync<TId>(TId id, CancellationToken ct = default);
    }
}
