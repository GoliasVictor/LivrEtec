namespace LivrEtec;

public interface IRepLivros
{
	IAsyncEnumerable<Livro> BuscarAsync(string nome, string nomeAutor, IEnumerable<Tag>? tags = null);
	Task EditarAsync(Livro livro);
	Task<Livro?> GetAsync(int id);
	Task RegistrarAsync(Livro livro);
	Task RemoverAsync(Livro livro);
}
