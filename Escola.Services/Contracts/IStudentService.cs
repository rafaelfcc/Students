using Escola.Domain.Contracts;
using Escola.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escola.Services.Records;

namespace Escola.Services.Contracts
{
    public interface IStudentService : _IBaseService<Student>
    {
        Task<Student?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<Student>> GetAllAsync(CancellationToken ct = default);

        Task<int> CreateAsync(StudentsInsertRecord input, CancellationToken ct = default);

        Task<bool> UpdateAsync(StudentsGenericRecord input, CancellationToken ct = default);

        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<StudentsImportResultRecord> ImportFromCsvAsync(Stream csvStream, CancellationToken ct = default);

    }
}
