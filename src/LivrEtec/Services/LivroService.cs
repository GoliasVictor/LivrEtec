
using Microsoft.Extensions.Logging;

namespace LivrEtec;

public class LivroService : Repositorio
{
	public LivroService(IAcervoService acervoService) : base(acervoService) { }

	public IQueryable<Livro> Buscar(string nome, string nomeAutor, IEnumerable<Tag> tags = null!)
	{

		IQueryable<Livro> livros = BD.Livros;
		if (!string.IsNullOrEmpty(nome) || !string.IsNullOrEmpty(nomeAutor))
		{
			var livPorNome = from livro in BD.Livros
							 where livro.Nome.Contains(nome)
							 select livro;
			var livPorAutor = from autor in BD.Autores
							  from livro in autor.Livros
							  where livro.Nome.Contains(nomeAutor)
							  select livro;
			livros = livPorNome.Union(livPorAutor);
		}


		if(tags is not null)
			foreach(var tag in tags )
				livros = livros.
					Where((livro) => livro.Tags.Contains(tag));
		Logger?.LogInformation($"Livros: Buscados; Parametros: nome: {nome}, Nome do autor {nomeAutor}, Tags: {string.Join(",",tags ?? Enumerable.Empty<Tag>())}");
		return livros;
	}

	public Livro? Get(int id) => BD.Livros.Find(id);
	public bool Registrar(Livro livro) {
		if ( string.IsNullOrWhiteSpace(livro.Nome)
		  || livro.Id < 0
		  || BD.Livros.Any((outro)=> outro.Id == livro.Id)
		)
			return false;
		
		BD.Livros.Add(livro);
		BD.SaveChanges();
		Logger?.LogInformation($"Livros: Registrado ${livro.Id}");
		return true;
	}

}