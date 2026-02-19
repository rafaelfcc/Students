namespace Escola.Services.Records
{
    public record StudentsGenericRecord
    (
        int Id,
        string Nome,
        int Idade,
        int Serie,
        double NotaMedia,
        string Endereco,
        string NomePai,
        string NomeMae,
        System.DateTime DataNascimento
    );
}
