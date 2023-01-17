namespace LivrEtec;

public interface IRepLivros
{
	Task<IEnumerable<Livro>> Buscar(string nome, string nomeAutor, IEnumerable<int>? idTags = null);
	Task Editar(Livro livro);
	Task<Livro?> Obter(int id);
	Task RegistrarObter(Livro livro);
	Task RemoverObter(int id);
}
