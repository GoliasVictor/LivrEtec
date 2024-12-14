using LivrEtec.GIB.RPC;

namespace LivrEtec.GIB.Services;

[Route("api/livros")]
[ApiController]
public sealed class LivrosServiceRPC 
{
    private readonly ILogger<LivrosServiceRPC> logger;
    private readonly ILivrosService livrosService;
    public LivrosServiceRPC(ILogger<LivrosServiceRPC> logger, ILivrosService livrosService)
    {
        this.logger = logger;
        this.livrosService = livrosService;
    }
    
    [HttpPost()]
    public async Task<Empty> Registrar(RPC.Livro request)
    {
        await livrosService.Registrar(request);
        return new Empty();

    }

    [HttpGet()]
    public async Task<RPC.Livro?> Obter(IdLivro request)
    {
        return await livrosService.Obter(request.Id);
    }

    [HttpDelete()]
    public async Task<Empty> Remover(IdLivro request)
    {
        await livrosService.Remover(request.Id);
        return new Empty();
    }

    [HttpGet("buscar")]
    public async Task<ListaLivros> Buscar(ParamBusca request)
    {
        IEnumerable<LEM::Livro> Livros = await livrosService.Buscar(request.NomeLivro, request.NomeAutor, request.IdTags);
        return new ListaLivros()
        {
            Livros = { Livros.Select(l => (RPC.Livro)l).ToArray() }
        };
    }

    [HttpPatch()]
    public async Task<Empty> Editar(RPC.Livro request)
    {
        await livrosService.Editar(request);
        return new Empty();
    }
}