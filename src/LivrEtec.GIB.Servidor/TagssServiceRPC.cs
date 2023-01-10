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
            try {
			    return new IdTag(){
                    Id = await tagsService.RegistrarAsync(request)
                };
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }

		}
		public override async Task<RPC.Tag?> Obter(IdTag request, ServerCallContext context)
        {
            try{
                return await tagsService.ObterAsync(request.Id);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
        }

        public override async Task<Empty> Remover(RPC.IdTag request, ServerCallContext context)
        {
            try {
                await tagsService.RemoverAsync(request.Id);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();
        }

        public override async Task<ListaTags> Buscar(BuscarRequest request, ServerCallContext context)
        {
            
            try{
				IEnumerable<Tag> Tags = await tagsService.BuscarAsync(request.Nome);
				return new ListaTags() { 
                    Tags = { Tags.Select(l=> (RPC.Tag)l).ToArray() }
                };
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
        }

        public override async Task<Empty> Editar(RPC.Tag request, ServerCallContext context)
        {
            try{
                await tagsService.EditarAsync(request);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();
        }
    }
}