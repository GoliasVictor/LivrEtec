using LivrEtec.Servidor.BD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace LivrEtec.Servidor.Repositorios;

public sealed class RepLivros : Repositorio, IRepLivros
{
    public RepLivros(PacaContext BD, ILogger<RepLivros> logger) : base(BD, logger)
    {
    }

    public async Task<IEnumerable<Livro>> Buscar(string nome, string nomeAutor, IEnumerable<int>? idTags)
    {
        IQueryable<Livro> livros = BD.Livros;

        if (!string.IsNullOrEmpty(nome) || !string.IsNullOrEmpty(nomeAutor))
        {
            IQueryable<Livro> livPorNome = from livro in BD.Livros
                                           where livro.Nome.Contains(nome)
                                           select livro;
            IQueryable<Livro> livPorAutor = from autor in BD.Autores
                                            from livro in autor.Livros
                                            where livro.Nome.Contains(nomeAutor)
                                            select livro;
            livros = livPorNome.Union(livPorAutor);
        }


        if (idTags is not null && idTags.Count() != 0)
        {
            foreach (var id in idTags)
            {
                livros = livros.Where((livro) => livro.Tags.Any((tag) => tag.Id == id));
            }
        }

        return await livros.ToListAsync();
    }
    private async Task<bool> ExisteAsync(Livro livro)
    {

        return await BD.Livros.ContainsAsync(livro);
    }
    private async Task<bool> ExisteAsync(int id)
    {

        return await BD.Livros.AnyAsync((l) => l.Id == id);
    }


    public async Task<Livro?> Obter(int id)
    {

        Livro? livro = await BD.Livros.FindAsync(id);
        if (livro == null)
        {
            return livro;
        }

        await BD.Entry(livro).Collection(l => l.Tags).LoadAsync();
        await BD.Entry(livro).Collection(l => l.Autores).LoadAsync();
        return livro;
    }

    public async Task RegistrarObter(Livro livro)
    {
        Validador.ErroSeInvalido(livro);
        if (await ExisteAsync(livro))
        {
            throw new InvalidOperationException($"O livro {{{livro.Id}}} já existe no sistema");
        }

        BD.AttachRange(livro.Tags);
        BD.AttachRange(livro.Autores);
        _ = BD.Livros.Add(livro);
        _ = await BD.SaveChangesAsync();
    }

    public async Task RemoverObter(int id)
    {

        if (await ExisteAsync(id) == false)
        {
            throw new InvalidOperationException($"O ID {{{id}}} já não existe no banco de dados");
        }

        _ = BD.Livros.Remove(BD.Livros.Find(id)!).Entity;
        _ = await BD.SaveChangesAsync();
    }

    public async Task Editar(Livro livro)
    {

        _ = livro ?? throw new ArgumentNullException(nameof(livro));
        foreach (Tag? tag in livro.Tags)
        {
            if (tag is null)
            {
                throw new InvalidDataException("tag nula");
            }
        }

        if (await ExisteAsync(livro) == false)
        {
            throw new InvalidOperationException($"Livro {{{livro.Nome}}} não existe no banco de dados");
        }

        Livro livroAntigo = BD.Livros.Include(l => l.Tags)
                                   .Include(l => l.Autores)
                                   .Single(l => l.Id == livro.Id);
        livroAntigo.Nome = livro.Nome;
        livroAntigo.Descricao = livro.Descricao;
        livroAntigo.Arquivado = livro.Arquivado;
        livroAntigo.Tags.Clear();
        livroAntigo.Autores.Clear();
        _ = await BD.SaveChangesAsync();
        livroAntigo.Autores.AddRange(BD.Autores.Where(a => livro.Autores.Contains(a)));
        livroAntigo.Tags.AddRange(BD.Tags.Where(t => livro.Tags.Contains(t)));
        _ = await BD.SaveChangesAsync();
    }
}