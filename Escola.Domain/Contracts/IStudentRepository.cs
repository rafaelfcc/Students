using Escola.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Escola.Domain.Contracts
{
    public interface IStudentRepository : _IBaseRepository<Student>
    {
        Task<Student?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<int> InsertAsync(Student student, CancellationToken ct = default);
        Task<bool> UpdateAsync(Student student, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
        Task<int> InsertManyAsync(List<Student> students, CancellationToken ct = default);
    }
}
