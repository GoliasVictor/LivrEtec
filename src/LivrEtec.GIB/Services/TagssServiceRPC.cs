namespace LivrEtec.GIB.Services;

[Route("api/tags")]
[ApiController]
public sealed class TagsServiceRPC
{
    private readonly ILogger<TagsServiceRPC> logger;
    private readonly ITagsService tagsService;
    public TagsServiceRPC(ILogger<TagsServiceRPC> logger, ITagsService tagsService)
    {
        this.logger = logger;
        this.tagsService = tagsService;
    }

    [HttpPost()]

    public async Task<int> Registrar(RPC::Tag request)
    {
        return await tagsService.Registrar(request);
    }
    [HttpGet("{id}")]
    public async Task<RPC::Tag?> Obter(int id)
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
    public async Task<IEnumerable<RPC::Tag>> Buscar([FromQuery]BuscarTagReq request)
    {
        return (await tagsService.Buscar(request.Nome)).Select(x => (RPC::Tag)x);
    }

    [HttpPut()]
    public async Task Editar(RPC::Tag request)
    {
        await tagsService.Editar(request);
    }
}