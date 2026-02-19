using Escola.Services.Records;
using Escola.Domain.Contracts;
using Escola.Domain.Entities;
using Escola.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;

namespace Escola.Services.Services
{
    public class StudentService : _BaseService<Student>, IStudentService
    {
        private readonly IStudentRepository _students;

        public StudentService(IStudentRepository students) : base(students)
        {
            _students = students;
        }

        public Task<Student?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0) throw new ArgumentException("Id inválido.");
            return _students.GetByIdAsync(id, ct);
        }

        public Task<List<Student>> GetAllAsync(CancellationToken ct = default)
            => _students.GetListAsync(_ => true, ct);

        public async Task<int> CreateAsync(StudentsInsertRecord input, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(input.Nome))
                throw new ArgumentException("Nome é obrigatório.");

            var student = new Student
            {
                Nome = input.Nome.Trim(),
                Idade = input.Idade,
                Serie = input.Serie,
                NotaMedia = input.NotaMedia,
                Endereco = input.Endereco?.Trim() ?? string.Empty,
                NomePai = input.NomePai?.Trim() ?? string.Empty,
                NomeMae = input.NomeMae?.Trim() ?? string.Empty,
                DataNascimento = input.DataNascimento
            };

            var id = await _students.InsertAsync(student, ct);
            return id;
        }

        public async Task<bool> UpdateAsync(StudentsGenericRecord input, CancellationToken ct = default)
        {
            if (input.Id <= 0) throw new ArgumentException("Id inválido.");
            if (string.IsNullOrWhiteSpace(input.Nome))
                throw new ArgumentException("Nome é obrigatório.");

            var student = new Student
            {
                Id = input.Id,
                Nome = input.Nome.Trim(),
                Idade = input.Idade,
                Serie = input.Serie,
                NotaMedia = input.NotaMedia,
                Endereco = input.Endereco?.Trim() ?? string.Empty,
                NomePai = input.NomePai?.Trim() ?? string.Empty,
                NomeMae = input.NomeMae?.Trim() ?? string.Empty,
                DataNascimento = input.DataNascimento
            };

            var ok = await _students.UpdateAsync(student, ct);
            return ok;
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");

            return _students.DeleteAsync(id, ct);
        }

        public async Task<StudentsImportResultRecord> ImportFromCsvAsync(Stream csvStream, CancellationToken ct = default)
        {
            if (csvStream == null)
                throw new ArgumentException("Stream do CSV inválida.");

            var errors = new List<StudentsImportErrorRecord>();
            var studentsToInsert = new List<Student>();

            using var parser = new TextFieldParser(csvStream)
            {
                TextFieldType = FieldType.Delimited,
                HasFieldsEnclosedInQuotes = true
            };

            parser.SetDelimiters(",");

            if (parser.EndOfData)
                return new StudentsImportResultRecord(0, 0, 0, new List<StudentsImportErrorRecord>  { new(0, "File", "CSV vazio.") });

            var header = parser.ReadFields();

            if (header == null || header.Length == 0)
                throw new ArgumentException("Cabeçalho do CSV inválido.");

            var map = BuildHeaderMap(header);

            int line = 1;

            while (!parser.EndOfData)
            {
                ct.ThrowIfCancellationRequested();
                line++;

                string[]? fields;
                try
                {
                    fields = parser.ReadFields();
                }
                catch (MalformedLineException)
                {
                    errors.Add(new StudentsImportErrorRecord(line, "Line", "Linha malformada no CSV."));
                    continue;
                }

                if (fields == null || fields.All(string.IsNullOrWhiteSpace))
                    continue;
                
                string Get(string key) =>
                    map.TryGetValue(key, out var idx) && idx >= 0 && idx < fields.Length
                        ? (fields[idx] ?? "").Trim()
                        : "";

                var nome = Get("nome");
                var idadeStr = Get("idade");
                var serieStr = Get("serie");
                var notaStr = Get("notamedia");
                var endereco = Get("endereco");
                var nomePai = Get("nomepai");
                var nomeMae = Get("nomemae");
                var dataNascStr = Get("datanascimento");

                if (string.IsNullOrWhiteSpace(nome))
                {
                    errors.Add(new StudentsImportErrorRecord(line, "Nome", "Nome é obrigatório."));
                    continue;
                }

                if (string.IsNullOrWhiteSpace(dataNascStr))
                {
                    errors.Add(new StudentsImportErrorRecord(line, "DataNascimento", "Data de Nascimento é obrigatória."));
                    continue;
                }

                if (!int.TryParse(idadeStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out var idade))
                    idade = 0;

                if (!int.TryParse(serieStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out var serie))
                    serie = 0;

                notaStr = (notaStr ?? "").Replace(',', '.');
                if (!double.TryParse(notaStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var notaMedia))
                    notaMedia = 0;

                if (!DateTime.TryParseExact(
                        dataNascStr,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var dtNasc))
                {
                    errors.Add(new StudentsImportErrorRecord(line, "DataNascimento", "Data de Nascimento inválida. Use YYYY-MM-DD."));
                    continue;
                }

                var dtUtc = DateTime.SpecifyKind(dtNasc.Date, DateTimeKind.Utc);

                studentsToInsert.Add(new Student
                {
                    Nome = nome.Trim(),
                    Idade = idade,
                    Serie = serie,
                    NotaMedia = notaMedia,
                    Endereco = endereco?.Trim() ?? string.Empty,
                    NomePai = nomePai?.Trim() ?? string.Empty,
                    NomeMae = nomeMae?.Trim() ?? string.Empty,
                    DataNascimento = dtUtc
                });
            }

            int imported = 0;
            if (studentsToInsert.Count > 0)
            {
                imported = await _students.InsertManyAsync(studentsToInsert, ct);
            }

            var total = (line - 1) - 1;
            var failed = errors.Count > 0 ? errors.Select(e => e.Line).Distinct().Count() : 0;

            return new StudentsImportResultRecord(
                Total: total < 0 ? 0 : total,
                Imported: imported,
                Failed: failed,
                Errors: errors
            );
        }

        private static Dictionary<string, int> BuildHeaderMap(string[] header)
        {
            var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < header.Length; i++)
            {
                var key = NormalizeHeader(header[i]);
                if (!string.IsNullOrWhiteSpace(key) && !map.ContainsKey(key))
                    map[key] = i;
            }

            return map;
        }

        private static string NormalizeHeader(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";

            s = s.Trim().ToLowerInvariant();

            var normalized = s.Normalize(NormalizationForm.FormD);
            var chars = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            s = new string(chars.ToArray()).Normalize(NormalizationForm.FormC);

            s = s.Replace(" ", "")
                 .Replace("-", "")
                 .Replace("_", "");

            if (s == "nomedopai")
                return "nomepai";

            if (s == "nomedamae")
                return "nomemae";

            if (s == "datadenascimento")
                return "datanascimento";

            return s;
        }
    }
}
