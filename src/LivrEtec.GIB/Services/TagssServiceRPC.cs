using LivrEtec.GIB.RPC;
using static LivrEtec.GIB.RPC.Tag.Types;

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

    public async Task<IdTag> Registrar(RPC.Tag request)
    {
        return new IdTag()
        {
            Id = await tagsService.Registrar(request)
        };

    }
    [HttpGet()]
    public async Task<RPC.Tag?> Obter(IdTag request)
    {
        return await tagsService.Obter(request.Id);
    }

    [HttpDelete()]
    public async Task<Empty> Remover(IdTag request)
    {
        await tagsService.Remover(request.Id);
        return new Empty();
    }

    public record BuscarTagReq(string Nome);
    [HttpGet("buscar")]
    public async Task<ListaTags> Buscar(BuscarTagReq request)
    {

        IEnumerable<LEM::Tag> Tags = await tagsService.Buscar(request.Nome);
        return new ListaTags()
        {
            Tags = { Tags.Select(l => (RPC.Tag)l).ToArray() }
        };
    }

    [HttpPatch()]
    public async Task<Empty> Editar(RPC.Tag request)
    {
        await tagsService.Editar(request);
        return new Empty();
    }
}