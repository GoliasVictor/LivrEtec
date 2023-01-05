namespace LivrEtec;

public interface IRepLivros
{
	Task<IEnumerable<Livro>> BuscarAsync(string nome, string nomeAutor, IEnumerable<int>? idTag = null);
	Task EditarAsync(Livro livro);
	Task<Livro?> GetAsync(int id);
	Task RegistrarAsync(Livro livro);
	Task RemoverAsync(int id);
}
