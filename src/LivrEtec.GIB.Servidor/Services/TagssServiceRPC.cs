using LivrEtec.GIB.RPC;
using static LivrEtec.GIB.RPC.Tag.Types;

namespace LivrEtec.GIB.Servidor.Services;

public sealed class TagsServiceRPC : Tags.TagsBase
{
    private readonly ILogger<TagsServiceRPC> logger;
    private readonly ITagsService tagsService;
    public TagsServiceRPC(ILogger<TagsServiceRPC> logger, ITagsService tagsService)
    {
        this.logger = logger;
        this.tagsService = tagsService;
    }


    public override async Task<Id> Registrar(RPC.Tag request, ServerCallContext context)
    {
        return new Id(await tagsService.Registrar(request));

    }
    public override async Task<RPC.Tag?> Obter(Id request, ServerCallContext context)
    {
        return await tagsService.Obter(request.Valor);
    }

    public override async Task<Empty> Remover(Id request, ServerCallContext context)
    {
        await tagsService.Remover(request.Valor);
        return new Empty();
    }

    public override async Task<ListaTags> Buscar(BuscarRequest request, ServerCallContext context)
    {

        IEnumerable<LEM::Tag> Tags = await tagsService.Buscar(request.Nome);
        return new ListaTags()
        {
            Tags = { Tags.Select(l => (RPC.Tag)l).ToArray() }
        };
    }

    public override async Task<Empty> Editar(RPC.Tag request, ServerCallContext context)
    {
        await tagsService.Editar(request);
        return new Empty();
    }
}