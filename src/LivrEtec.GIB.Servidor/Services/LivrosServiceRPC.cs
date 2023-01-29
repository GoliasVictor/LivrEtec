using LivrEtec.GIB.RPC;

namespace LivrEtec.GIB.Servidor.Services;

public sealed class LivrosServiceRPC : Livros.LivrosBase
{
    private readonly ILogger<LivrosServiceRPC> logger;
    private readonly ILivrosService livrosService;
    public LivrosServiceRPC(ILogger<LivrosServiceRPC> logger, ILivrosService livrosService)
    {
        this.logger = logger;
        this.livrosService = livrosService;
    }


    public override async Task<Empty> Registrar(RPC.Livro request, ServerCallContext context)
    {
        await livrosService.Registrar(request);
        return new Empty();

    }
    public override async Task<RPC.Livro?> Obter(IdLivro request, ServerCallContext context)
    {
        return await livrosService.Obter(request.Id);
    }

    public override async Task<Empty> Remover(IdLivro request, ServerCallContext context)
    {
        await livrosService.Remover(request.Id);
        return new Empty();
    }

    public override async Task<ListaLivros> Buscar(ParamBusca request, ServerCallContext context)
    {
        IEnumerable<LEM::Livro> Livros = await livrosService.Buscar(request.NomeLivro, request.NomeAutor, request.IdTags);
        return new ListaLivros()
        {
            Livros = { Livros.Select(l => (RPC.Livro)l).ToArray() }
        };
    }

    public override async Task<Empty> Editar(RPC.Livro request, ServerCallContext context)
    {
        await livrosService.Editar(request);
        return new Empty();
    }
}