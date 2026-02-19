using Escola.API.DTO;
using Escola.Domain.Contracts;
using Escola.Domain.Entities;
using Escola.Services.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _users;

        public UsersController(IUserService users)
        {
            _users = users;
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<UserReadDTO>> GetById(int id, CancellationToken ct)
        {
            if (id <= 0) throw new ArgumentException("Id inválido.");

            var user = await _users.GetByIdAsync(id, ct);
            if (user is null) throw new KeyNotFoundException("Usuário não encontrado.");

            return Ok(new UserReadDTO
            {
                Id = user.Id,
                Username = user.Username
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserInsertDTO dto, CancellationToken ct)
        {
            if (dto is null) throw new ArgumentException("Body é obrigatório.");

            var id = await _users.CreateAsync(
                new UserCreateRecord(dto.Username, dto.Password),
                ct);

            var readDto = new UserReadDTO { Id = id, Username = dto.Username.Trim() };
            return CreatedAtAction(nameof(GetById), new { id }, readDto);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDTO dto, CancellationToken ct)
        {
            if (dto is null)
                throw new ArgumentException("Body é obrigatório.");

            if (id != dto.Id)
                throw new ArgumentException("O Id da rota deve ser igual ao Id do body.");

            await _users.ChangePasswordAsync(
                new UserChangePasswordRecord() { UserId = dto.Id, UserName = dto.Username, PrevPassword = dto.PrevPassword, NewPassword = dto.NewPassword }, ct);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _users.DeleteAsync(id, ct);
            if (!ok) throw new KeyNotFoundException("Usuário não encontrado.");
            return NoContent();
        }
    }
}
