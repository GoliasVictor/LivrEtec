using LivrEtec.Servidor.BD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace LivrEtec.Servidor.Repositorios;

public sealed class RepTags : Repositorio, IRepTags
{
    public RepTags(PacaContext BD, ILogger<RepTags> logger) : base(BD, logger)
    {
    }

    public async Task<IEnumerable<Tag>> Buscar(string nome)
    {
        return string.IsNullOrEmpty(nome)
            ? await BD.Tags.ToListAsync()
            : (IEnumerable<Tag>)await BD.Tags.Where(tag => EF.Functions.Like(tag.Nome, $"%{nome}%")).ToListAsync();
    }
    private async Task<bool> ExisteAsync(int id)
    {
        return await BD.Tags.AnyAsync((l) => l.Id == id);
    }


    public async Task<Tag?> Obter(int id)
    {
        return await BD.Tags.FindAsync(id);
    }

    public async Task<int> Registrar(Tag tag)
    {
        Validador.ErroSeInvalido(tag);
        if (await ExisteAsync(tag.Id))
        {
            throw new InvalidOperationException($"O tag {{{tag.Id}}} já existe no sistema");
        }

        _ = BD.Tags.Add(tag);
        _ = await BD.SaveChangesAsync();
        return tag.Id;
    }

    public async Task Remover(int id)
    {

        if (await ExisteAsync(id) == false)
        {
            throw new InvalidOperationException($"O ID {{{id}}} já não existe no banco de dados");
        }

        _ = BD.Tags.Remove(BD.Tags.Find(id)!).Entity;
        _ = await BD.SaveChangesAsync();
    }

    public async Task Editar(Tag tag)
    {
        _ = tag ?? throw new ArgumentNullException(nameof(tag));
        Tag? tagAntiga = await BD.Tags.SingleOrDefaultAsync(t => t.Id == tag.Id);
        _ = tagAntiga ?? throw new InvalidOperationException($"tag {{{tag.Id}}} não existe no banco de dados");
        tagAntiga.Nome = tag.Nome;
        _ = await BD.SaveChangesAsync();
    }
}