using Grpc.Core;
using LivrEtec.GIB.RPC;

namespace LivrEtec.GIB.Servidor
{
    public sealed class LivrosServiceRPC : Livros.LivrosBase
    {
        readonly ILogger<LivrosServiceRPC> logger;
        readonly ILivrosService livrosService;
        public LivrosServiceRPC(ILogger<LivrosServiceRPC> logger, ILivrosService livrosService)
        {
            this.logger = logger;
            this.livrosService = livrosService;
        }


        public override async Task<Empty> Registrar(RPC.Livro request, ServerCallContext context)
		{
            try {
			    await livrosService.RegistrarAsync(request);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();

		}
		public override async Task<RPC.Livro?> Get(IdLivro request, ServerCallContext context)
        {
            try{
                return await livrosService.GetAsync(request.Id);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
        }

        public override async Task<Empty> Remover(RPC.IdLivro request, ServerCallContext context)
        {
            try {
                await livrosService.RemoverAsync(request.Id);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();
        }

        public override async Task<ListaLivros> Buscar(ParamBusca request, ServerCallContext context)
        {
            
            try{
				IEnumerable<Livro> Livros = await livrosService.BuscarAsync(request.NomeLivro, request.NomeAutor, request.IdTags);
				return new ListaLivros() { 
                    Livros = { Livros.Select(l=> (RPC.Livro)l).ToArray() }
                };
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
        }

        public override async Task<Empty> Editar(RPC.Livro request, ServerCallContext context)
        {
            try{
                await livrosService.EditarAsync(request);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();
        }
    }
}