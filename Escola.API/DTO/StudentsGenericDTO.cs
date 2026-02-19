using System.ComponentModel.DataAnnotations;

namespace Escola.API.DTO
{
    public class StudentsGenericDTO
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; } = 0;
        public int Serie { get; set; } = 0;
        public double NotaMedia { get; set; }
        public string Endereco { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;
        public string NomeMae { get; set; } = string.Empty;
        public System.DateTime DataNascimento { get; set; } = DateTime.MinValue;
    }
}
