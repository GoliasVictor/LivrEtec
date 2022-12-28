
namespace LivrEtec
{
    public interface IEmprestimo
    {
        Pessoa Pessoa { get; set; }
        Livro Livro { get; set; }
        string? Comentario { get; set; } 
        DateTime? DataFechamento { get; set; }
        DateTime DataEmprestimo { get; set; }
        DateTime FimDataEmprestimo { get; set; }
    }
}