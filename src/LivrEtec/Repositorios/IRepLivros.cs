namespace LivrEtec;

public interface IRepLivros
{
	IEnumerable<Livro> Buscar(string nome, string nomeAutor, IEnumerable<Tag>? tags = null);
	void Editar(Livro livro);
	Livro? Get(int id);
	void Registrar(Livro livro);
	void Remover(Livro livro);
}
