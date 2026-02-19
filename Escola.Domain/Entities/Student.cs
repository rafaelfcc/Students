using System;

namespace Escola.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public int Serie { get; set; }
        public double NotaMedia { get; set; }
        public string Endereco { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;
        public string NomeMae { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
    }
}
