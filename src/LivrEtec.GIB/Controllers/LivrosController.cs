
namespace LivrEtec.GIB.Controllers;

[Route("livros")]
[ApiController]
public sealed class LivrosController(ILogger<LivrosController> logger, ILivrosService livrosService)
{
    private readonly ILogger<LivrosController> logger = logger;
    private readonly ILivrosService livrosService = livrosService;

    [HttpPost()]
    public async Task Registrar(DTO::Livro request)
    {
        await livrosService.Registrar(request);
    }

    [HttpGet("{id}")]
    public async Task<DTO::Livro?> Obter(int id)
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
    public async Task<IEnumerable<DTO::Livro>> Buscar([FromQuery]ParamBuscaLivro request)
    {
        return (await livrosService.Buscar(request.NomeLivro ?? "", request.NomeAutor ?? "", request.IdTags))
            .Select(l =>  (DTO::Livro)l);
    }

    [HttpPut()]
    public async Task Editar(DTO::Livro request)
    {
        Console.WriteLine(request);
        await livrosService.Editar(request);
    }
}