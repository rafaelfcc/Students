using Escola.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Escola.Services.Services
{
    public abstract class _BaseService<T> : _IBaseService<T> where T : class
    {
        protected readonly _IBaseRepository<T> _repository;

        protected _BaseService(_IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual Task<T?> GetByIdAsync<TId>(TId id, CancellationToken ct = default)
        {
            return _repository.GetByIdAsync(id, ct);
        }

        public virtual Task<List<T>> GetListAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken ct = default)
        {
            return _repository.GetListAsync(filter, ct);
        }

        public virtual Task<TId> CreateAsync<TId>(T item, CancellationToken ct = default)
        {
            return _repository.InsertAsync<TId>(item, ct);
        }

        public virtual Task<bool> UpdateAsync(T item, CancellationToken ct = default)
        {
            return _repository.UpdateAsync(item, ct);
        }

        public virtual Task<bool> DeleteAsync<TId>(TId id, CancellationToken ct = default)
        {
            return _repository.DeleteAsync(id, ct);
        }
    }
}
