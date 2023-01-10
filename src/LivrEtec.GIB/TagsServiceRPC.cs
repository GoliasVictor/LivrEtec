using LivrEtec.GIB;
using RPC = LivrEtec.GIB.RPC;
using Microsoft.Extensions.Logging;
using LivrEtec.GIB.RPC;
using Grpc.Core;
using static LivrEtec.GIB.RPC.Tag.Types;
namespace LivrEtec.GIB
{
    public sealed class TagsServiceRPC:  ITagsService
    {
        readonly ILogger<TagsServiceRPC> logger;
        readonly RPC::Tags.TagsClient tagsClientRPC;
        public TagsServiceRPC(RPC::Tags.TagsClient tagsClientRPC, ILogger<TagsServiceRPC> logger)
        {
            this.tagsClientRPC = tagsClientRPC;
            this.logger = logger;
        }
        public async Task<int> RegistrarAsync(Tag tag)
        { 
            Validador.ErroSeInvalido(tag);
            try{
               return (await tagsClientRPC.RegistrarAsync(tag)).Id;
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }

		}

        public async Task EditarAsync(Tag tag)
        {
            _ = tag ?? throw new ArgumentNullException(nameof(tag));
            try{
                await tagsClientRPC.EditarAsync(tag);
            }
            catch(RpcException ex){
               throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

        public async Task<Tag?> ObterAsync(int id)
        {
            try{
                return await tagsClientRPC.ObterAsync(new IdTag() { Id = id });
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex); 
            }
        }


		public async Task RemoverAsync(int id)
        {
            try{
                await tagsClientRPC.RemoverAsync(new IdTag(){ Id = id});
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

        public async Task<IEnumerable<Tag>> BuscarAsync(string nome)
        {
            nome ??= "";
            try{
			    ListaTags listaTags = await tagsClientRPC.BuscarAsync(new BuscarRequest(){ Nome = nome});
			    return listaTags.Tags.Select(l=> (Tag)l!);
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

	}
}