namespace LivrEtec.GIB.Services;

[Route("api/tags")]
[ApiController]
public sealed class TagsController(ILogger<TagsController> logger, ITagsService tagsService)
{
    private readonly ILogger<TagsController> logger = logger;
    private readonly ITagsService tagsService = tagsService;

    [HttpPost()]
    public async Task<int> Registrar(DTO::Tag request)
    {
        return await tagsService.Registrar(request);
    }
    [HttpGet("{id}")]
    public async Task<DTO::Tag?> Obter(int id)
    {
        return await tagsService.Obter(id);
    }

    [HttpDelete("{id}")]
    public async Task Remover(int id)
    {
        await tagsService.Remover(id);
    }

    public record BuscarTagReq(string Nome);
    [HttpGet()]
    public async Task<IEnumerable<DTO::Tag>> Buscar([FromQuery]BuscarTagReq request)
    {
        return (await tagsService.Buscar(request.Nome)).Select(x => (DTO::Tag)x);
    }

    [HttpPut()]
    public async Task Editar(DTO::Tag request)
    {
        await tagsService.Editar(request);
    }
}