using LivrEtec.Servidor.BD;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.Repositorios;

public sealed class RepAutores : Repositorio, IRepAutores
{
    public RepAutores(PacaContext BD, ILogger<RepAutores> logger) : base(BD, logger)
    {
    }

    public async Task Registrar(Autor autor)
    {
        Validador.ErroSeInvalido(autor);
        _ = BD.Autores.Add(autor);
        _ = await BD.SaveChangesAsync();
        logger?.LogInformation($"Autor @{autor.Id} registrado");
    }

    public IAsyncEnumerable<Autor> Todos()
    {
        IAsyncEnumerable<Autor> Autores = BD.Autores.AsAsyncEnumerable();
        logger?.LogInformation("Todos Autores Coletados");
        return Autores;
    }
}