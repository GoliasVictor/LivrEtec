using LivrEtec.Servidor;

namespace LivrEtec;

public interface IAcervoService
{
	IRepLivros Livros { get; init; }
    IRepPessoas Pessoas { get; init; }
    IRepEmprestimo Emprestimos { get; init; }
    IRepUsuarios Usuarios { get; init; }
}
