using Grpc.Core;
using LivrEtec.GIB.RPC;
using static LivrEtec.GIB.RPC.Tag.Types;

namespace LivrEtec.GIB.Servidor
{
	public sealed class TagsServiceRPC : Tags.TagsBase
	{
		readonly ILogger<TagsServiceRPC> logger;
		readonly ITagsService tagsService;
		public TagsServiceRPC(ILogger<TagsServiceRPC> logger, ITagsService tagsService)
		{
			this.logger = logger;
			this.tagsService = tagsService;
		}


		public override async Task<IdTag> Registrar(RPC.Tag request, ServerCallContext context)
		{
			return new IdTag()
			{
				Id = await tagsService.Registrar(request)
			};

		}
		public override async Task<RPC.Tag?> Obter(IdTag request, ServerCallContext context)
		{
			return await tagsService.Obter(request.Id);
		}

		public override async Task<Empty> Remover(RPC.IdTag request, ServerCallContext context)
		{
			await tagsService.Remover(request.Id);
			return new Empty();
		}

		public override async Task<ListaTags> Buscar(BuscarRequest request, ServerCallContext context)
		{

			IEnumerable<Tag> Tags = await tagsService.Buscar(request.Nome);
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
}