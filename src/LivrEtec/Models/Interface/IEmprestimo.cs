
namespace LivrEtec
{
    public interface IEmprestimo
    {
        Aluno? Aluno { get; set; }
        Livro? Livro { get; set; }
        string? Comentario { get; set; }
        DateTime DataDevolucao { get; set; }
        DateTime DataEmprestimo { get; set; }
        DateTime FimDataEmprestimo { get; set; }
    }
}