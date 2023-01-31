using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.Services;

public sealed class LivrosService : ILivrosService
{
    private readonly ILogger<LivrosService>? Logger;
    private readonly IIdentidadeService identidadeService;
    private readonly IRepLivros repLivros;
    public LivrosService(
        IRepLivros repLivros,
        IIdentidadeService identidadeService,
        ILogger<LivrosService>? logger
    )
    {
        this.repLivros = repLivros;
        this.identidadeService = identidadeService;
        Logger = logger;
    }

    public async Task<IEnumerable<Livro>> Buscar(string nome, string nomeAutor, IEnumerable<int>? idTags = null)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Livro.Visualizar);
        IEnumerable<Livro> livros = await repLivros.Buscar(nome, nomeAutor, idTags);
        Logger?.LogInformation("Livros buscados, Detalhes: nome={{{nome}}}; nomeAutor={{{nomeAutor}}}; idTags={{{idTags}}}",
            nome,
            nomeAutor,
            idTags != null ? string.Join(",", idTags) : null
        );
        return livros;
    }

    public async Task Editar(Livro livro)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Livro.Editar);
        await repLivros.Editar(livro);
        Logger?.LogInformation(1, "Livro Editado, Detalhes: livro={{{nome}}};", livro);
    }

    public async Task<Livro?> Obter(int id)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Livro.Visualizar);
        Livro? livro = await repLivros.Obter(id);
        Logger?.LogInformation(1, "Livro obtido, Detalhes: id={{{id}}};", id);
        return livro;
    }

    public async Task Registrar(Livro livro)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Livro.Criar);
        await repLivros.RegistrarObter(livro);
        Logger?.LogInformation(1, "Livro registrado, Detalhes: livro={{{livro}}};", livro);
    }

    public async Task Remover(int id)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Livro.Excluir);
        await repLivros.RemoverObter(id);
        Logger?.LogInformation(1, "Livro excluid, Detalhes: id={{{id}}};", id);
    }
}
