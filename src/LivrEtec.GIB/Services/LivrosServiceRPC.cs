
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
    public async Task Registrar(RPC::Livro request)
    {
        await livrosService.Registrar(request);
    }

    [HttpGet("{id}")]
    public async Task<RPC::Livro?> Obter(int id)
    {
        return await livrosService.Obter(id);
    }

    [HttpDelete("{id}")]
    public async Task Remover(int id)
    {
        await livrosService.Remover(id);
    }
    public record ParamBuscaLivro(string? NomeLivro, string? NomeAutor, IEnumerable<int>? IdTags);
    [HttpGet()]
    public async Task<IEnumerable<RPC::Livro>> Buscar([FromQuery]ParamBuscaLivro request)
    {
        return (await livrosService.Buscar(request.NomeLivro ?? "", request.NomeAutor ?? "", request.IdTags))
            .Select(l =>  (RPC::Livro)l);
    }

    [HttpPut()]
    public async Task Editar(RPC::Livro request)
    {
        Console.WriteLine(request);
        await livrosService.Editar(request);
    }
}