namespace LivrEtec;

public interface IRepLivros
{
	IEnumerable<Livro> Buscar(string nome, string nomeAutor, IEnumerable<Tag>? tags = null);
	IAsyncEnumerable<Livro> BuscarAsync(string nome, string nomeAutor, IEnumerable<Tag>? tags = null);
	void Editar(Livro livro);
	Task EditarAsync(Livro livro);
	Livro? Get(int id);
	Task<Livro?> GetAsync(int id);
	void Registrar(Livro livro);
	Task RemoverAsync(Livro livro);
	void Remover(Livro livro);
}
