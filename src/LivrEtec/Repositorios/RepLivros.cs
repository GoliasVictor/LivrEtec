
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Data.Entity.Infrastructure;

namespace LivrEtec;

public sealed class RepLivros : Repositorio, IRepLivros
{
	public RepLivros(AcervoService acervoService) : base(acervoService) { }

	public IQueryable<Livro> Buscar(string nome, string nomeAutor, IEnumerable<Tag>? tags = null)
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


		if (tags is not null)
			foreach (var tag in tags)
				livros = livros.
					Where((livro) => livro.Tags.Contains(tag));
		Logger?.LogInformation($"Livros: Buscados; Parametros: nome: {nome}, Nome do autor {nomeAutor}, Tags: {string.Join(",", tags ?? Enumerable.Empty<Tag>())}");
		return livros;
	}

	public Livro? Get(int id)
	{

		var livro = BD.Livros.Find(id);
		if (livro == null)
			return livro;
		BD.Entry(livro).Collection(l => l.Tags).Load();
		BD.Entry(livro).Collection(l => l.Autores).Load();
		return livro;
	}
	public bool Registrar(Livro livro)
	{
		if (string.IsNullOrWhiteSpace(livro.Nome)
		  || livro.Id < 0
		  || BD.Livros.Any((outro) => outro.Id == livro.Id)
		)
			return false;

		BD.Livros.Add(livro);
		BD.SaveChanges();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} registrado");
		return true;
	}
	public void Remover(Livro livro)
	{
		if(!BD.Livros.Contains(livro))
			throw new ArgumentException($"Livro {{{livro.Nome}}} já não existe no banco de dados");
		BD.Livros.Remove(livro);
		BD.SaveChanges();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} excluido");
	}
	public void Editar(Livro livro)
	{
		if(!BD.Livros.Contains(livro))
			throw new ArgumentException($"Livro {{{livro.Nome}}} não existe no banco de dados");
		BD.Livros.Update(livro);
		BD.SaveChanges();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} editado");
	}
}