
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Data;

namespace LivrEtec.Servidor;

public sealed class RepLivros : Repositorio, IRepLivros
{
	public RepLivros(AcervoService acervoService) : base(acervoService) 
	{	
	}
 
	public async Task<IEnumerable<Livro>> BuscarAsync(string nome, string nomeAutor, IEnumerable<Tag>? tags)
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


		if (tags is not null && tags.Count() != 0)
			foreach (var tag in tags)
				livros = livros.Where((livro) => livro.Tags.Contains(tag));
				
		Logger?.LogInformation($"Livros: Buscados; Parametros: nome: {nome}, Nome do autor {nomeAutor}, Tags: {string.Join(",", tags ?? Enumerable.Empty<Tag>())}");
		return await livros.ToListAsync();
	}
	private async Task<bool> ExisteAsync(Livro livro)
	{

		return await BD.Livros.ContainsAsync(livro);
	}
	private async Task<bool> ExisteAsync(int id)
	{

		return await BD.Livros.AnyAsync((l)=> l.Id == id);
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

	public async Task RegistrarAsync(Livro livro)
	{

		_= livro ?? throw new ArgumentNullException(nameof(livro));
		if (string.IsNullOrWhiteSpace(livro.Nome) || livro.Id < 0)
			throw new InvalidDataException();
		if (await ExisteAsync(livro))
			throw new InvalidOperationException($"O livro {{{ livro.Id }}} já existe no sistema");
		BD.AttachRange(livro.Tags);
		BD.AttachRange(livro.Autores);
		BD.Livros.Add(livro); 
		await BD.SaveChangesAsync();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} registrado");
	}

	public async Task RemoverAsync(int id)
	{

		if(await ExisteAsync(id) == false)
			throw new InvalidOperationException($"O ID {{{id}}} já não existe no banco de dados");
		var livro = BD.Livros.Remove(BD.Livros.Find(id)!).Entity;
		await BD.SaveChangesAsync();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} excluido");
	}

	public async Task EditarAsync(Livro livro)
	{

		_= livro ?? throw new ArgumentNullException(nameof(livro));
	   	foreach(var tag in livro.Tags) 
			if (tag is null)
				throw new InvalidDataException("tag nula");
		if( await ExisteAsync(livro) == false)
			throw new InvalidOperationException($"Livro {{{livro.Nome}}} não existe no banco de dados");
		
		var livroAntigo = BD.Livros.Include(l=>l.Tags)
								   .Include(l => l.Autores)
								   .Single(l => l.Id == livro.Id);
		livroAntigo.Nome = livro.Nome;
		livroAntigo.Descricao = livro.Descricao;
		livroAntigo.Arquivado = livro.Arquivado;
		livroAntigo.Tags.Clear();
		livroAntigo.Autores.Clear();
        await  BD.SaveChangesAsync();
		livroAntigo.Autores.AddRange(BD.Autores.Where( a=> livro.Autores.Contains(a)));
        livroAntigo.Tags.AddRange(BD.Tags.Where( t => livro.Tags.Contains(t)));
		await  BD.SaveChangesAsync();
		Logger?.LogInformation($"Livro {{{livro.Id}}} de nome {{{livro.Nome}}} editado");
	}
}