
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace LivrEtec.Servidor;

public sealed class RepLivros : Repositorio, IRepLivros
{
	public RepLivros(AcervoService acervoService) : base(acervoService) { }
	IQueryable<Livro> queryBuscar(string nome, string nomeAutor, IEnumerable<Tag>? tags = null){
		
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


		if (tags is not null && tags.Count() != 0)
			foreach (var tag in tags)
				livros = livros.
					Where((livro) => livro.Tags.Contains(tag));
		return livros;
	}
	public IEnumerable<Livro> Buscar(string nome, string nomeAutor, IEnumerable<Tag>? tags = null)
	{
		IQueryable<Livro> livros = queryBuscar(nome, nomeAutor, tags);
		Logger?.LogInformation($"Livros: Buscados; Parametros: nome: {nome}, Nome do autor {nomeAutor}, Tags: {string.Join(",", tags ?? Enumerable.Empty<Tag>())}");
		return livros;
	}
	public IAsyncEnumerable<Livro> BuscarAsync(string nome, string nomeAutor, IEnumerable<Tag>? tags)
	{
		IQueryable<Livro> livros = queryBuscar(nome, nomeAutor, tags);
		Logger?.LogInformation($"Livros: Buscados; Parametros: nome: {nome}, Nome do autor {nomeAutor}, Tags: {string.Join(",", tags ?? Enumerable.Empty<Tag>())}");
		return  livros.AsAsyncEnumerable();
	}
	private bool Existe(Livro livro)
	{
		return BD.Livros.Contains(livro);
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
	public async Task<Livro?> GetAsync(int id)
	{
		var livro = await BD.Livros.FindAsync(id);
		if (livro == null)
			return livro;
		await BD.Entry(livro).Collection(l => l.Tags).LoadAsync();
		await BD.Entry(livro).Collection(l => l.Autores).LoadAsync();
		return livro;
	}

	public void Registrar(Livro livro)
	{
		_= livro ?? throw new ArgumentNullException(nameof(livro));
		if (string.IsNullOrWhiteSpace(livro.Nome) || livro.Id < 0)
			throw new InvalidDataException();
		if (Existe(livro))
			throw new InvalidOperationException($"O livro {{{ livro.Id }}} já existe no sistema");
		BD.Livros.Add(livro); 
		BD.SaveChanges();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} registrado");
	}

	public async Task RegistrarAsync(Livro livro)
	{
		_= livro ?? throw new ArgumentNullException(nameof(livro));
		if (string.IsNullOrWhiteSpace(livro.Nome) || livro.Id < 0)
			throw new InvalidDataException();
		if (Existe(livro))
			throw new InvalidOperationException($"O livro {{{ livro.Id }}} já existe no sistema");
		BD.Livros.Add(livro); 
		await BD.SaveChangesAsync();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} registrado");
	}

	public void Remover(Livro livro)
	{
		_= livro ?? throw new ArgumentNullException(nameof(livro));
		if(!Existe(livro))
			throw new InvalidOperationException($"Livro {{{livro.Nome}}} já não existe no banco de dados");
		BD.Livros.Remove(livro);
		BD.SaveChanges();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} excluido");
	}
	public async Task RemoverAsync(Livro livro)
	{
		_= livro ?? throw new ArgumentNullException(nameof(livro));
		if(!Existe(livro))
			throw new InvalidOperationException($"Livro {{{livro.Nome}}} já não existe no banco de dados");
		BD.Livros.Remove(livro);
		await BD.SaveChangesAsync();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} excluido");
	}
	public void Editar(Livro livro)
	{
		_= livro ?? throw new ArgumentNullException(nameof(livro));
		if(!Existe(livro))
			throw new InvalidOperationException($"Livro {{{livro.Nome}}} não existe no banco de dados");
		BD.Livros.Update(livro);
		BD.SaveChanges();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} editado");
	}

	public async Task EditarAsync(Livro livro)
	{

		_= livro ?? throw new ArgumentNullException(nameof(livro));
		if(!Existe(livro))
			throw new InvalidOperationException($"Livro {{{livro.Nome}}} não existe no banco de dados");
		BD.Livros.Update(livro);
		await  BD.SaveChangesAsync();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} editado");
	}





}