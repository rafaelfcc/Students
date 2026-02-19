using Escola.API.DTO;
using Escola.Domain.Entities;
using Escola.Services.Contracts;
using Escola.Services.Records;
using Escola.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers
{
    [Route("api/students")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _students;

        public StudentsController(IStudentService students)
        {
            _students = students;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentsGenericDTO>> GetById(int id, CancellationToken ct)
        {
            var student = await _students.GetByIdAsync(id, ct);
            if (student is null) throw new KeyNotFoundException("Aluno não encontrado.");

            return Ok(ToGenericDTO(student));
        }

        [HttpGet]
        public async Task<ActionResult<List<StudentsListingDTO>>> GetAll(CancellationToken ct)
        {
            var list = await _students.GetAllAsync(ct);

            var dtos = list.Select(s => new StudentsListingDTO
            {
                Id = s.Id,
                Nome = s.Nome
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentsInsertDTO dto, CancellationToken ct)
        {
            if (dto is null) throw new ArgumentException("Payload é obrigatório.");

            var id = await _students.CreateAsync(
                new StudentsInsertRecord(
                    dto.Nome,
                    dto.Idade,
                    dto.Serie,
                    dto.NotaMedia,
                    dto.Endereco,
                    dto.NomePai,
                    dto.NomeMae,
                    dto.DataNascimento
                ),
                ct);

            var created = new StudentsGenericDTO
            {
                Id = id,
                Nome = dto.Nome?.Trim() ?? string.Empty,
                Idade = dto.Idade,
                Serie = dto.Serie,
                NotaMedia = dto.NotaMedia,
                Endereco = dto.Endereco ?? string.Empty,
                NomePai = dto.NomePai ?? string.Empty,
                NomeMae = dto.NomeMae ?? string.Empty,
                DataNascimento = dto.DataNascimento
            };

            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentsGenericDTO dto, CancellationToken ct)
        {
            if (dto is null) throw new ArgumentException("Body é obrigatório.");
            if (id != dto.Id) throw new ArgumentException("O Id da rota deve ser igual ao Id do body.");

            var ok = await _students.UpdateAsync(
                new StudentsGenericRecord(
                    dto.Id,
                    dto.Nome,
                    dto.Idade,
                    dto.Serie,
                    dto.NotaMedia,
                    dto.Endereco,
                    dto.NomePai,
                    dto.NomeMae,
                    dto.DataNascimento
                ), ct);

            if (!ok) throw new KeyNotFoundException("Aluno não encontrado.");

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _students.DeleteAsync(id, ct);
            if (!ok) throw new KeyNotFoundException("Aluno não encontrado.");
            return NoContent();
        }

        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportCsv([FromForm] IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Arquivo CSV não informado." });

            if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { message = "Envie um arquivo .csv" });

            using var stream = file.OpenReadStream();

            var result = await _students.ImportFromCsvAsync(stream, ct);

            return Ok(result);
        }

        private static StudentsGenericDTO ToGenericDTO(Student s) => new StudentsGenericDTO
        {
            Id = s.Id,
            Nome = s.Nome,
            Idade = s.Idade,
            Serie = s.Serie,
            NotaMedia = s.NotaMedia,
            Endereco = s.Endereco,
            NomePai = s.NomePai,
            NomeMae = s.NomeMae,
            DataNascimento = s.DataNascimento
        };
    }
}
