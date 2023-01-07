namespace LivrEtec;

public interface ILivrosService
{
	Task<IEnumerable<Livro>> BuscarAsync(string nome, string nomeAutor, IEnumerable<int>? idTags = null);
	Task EditarAsync(Livro livro);
	Task<Livro?> GetAsync(int id);
	Task RegistrarAsync(Livro livro);
	Task RemoverAsync(int id);
}
