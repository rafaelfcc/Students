using Escola.Data.Context;
using Escola.Domain.Contracts;
using Escola.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Escola.Repositories.Repositories
{
    public class StudentRepository : _BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(DataContext context) : base(context) { }

        public Task<Student?> GetByIdAsync(int id, CancellationToken ct = default)
            => base.GetByIdAsync(id, ct);

        public async Task<int> InsertAsync(Student student, CancellationToken ct = default)
        {
            return await base.InsertAsync<int>(student, ct);
        }

        public override Task<bool> UpdateAsync(Student student, CancellationToken ct = default)
            => base.UpdateAsync(student, ct);

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
            => base.DeleteAsync(id, ct);

        public async Task<int> InsertManyAsync(List<Student> students, CancellationToken ct = default)
        {
            if (students == null || students.Count == 0) return 0;

            await using var tx = await _context.Database.BeginTransactionAsync(ct);

            await _context.Set<Student>().AddRangeAsync(students, ct);

            var affected = await _context.SaveChangesAsync(ct);

            await tx.CommitAsync(ct);

            return students.Count;
        }

    }
}
