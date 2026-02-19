namespace Escola.API.DTO
{
    public class StudentsInsertDTO
    {
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; } = 0;
        public int Serie { get; set; } = 0;
        public double NotaMedia { get; set; } = 0;
        public string Endereco { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;
        public string NomeMae { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; } = DateTime.MinValue;
    }
}
