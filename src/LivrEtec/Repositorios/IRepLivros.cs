namespace LivrEtec;

public interface IRepLivros
{
	IQueryable<Livro> Buscar(string nome, string nomeAutor, IEnumerable<Tag>? tags = null);
	void Editar(Livro livro);
	Livro? Get(int id);
	bool Registrar(Livro livro);
	void Remover(Livro livro);
}
