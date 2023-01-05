using Grpc.Core;
using LivrEtec.GIB.RPC;

namespace LivrEtec.GIB.Servidor
{
    public sealed class LivroServiceRPC : Livros.LivrosBase
    {
        readonly ILogger<LivroServiceRPC> logger;
        readonly IRepLivros repLivros;
        public LivroServiceRPC(ILogger<LivroServiceRPC> logger, IRepLivros repLivros)
        {
            this.logger = logger;
            this.repLivros = repLivros;
        }


        public override async Task<Empty> Registrar(RPC.Livro request, ServerCallContext context)
		{
            try {
			    await repLivros.RegistrarAsync(request);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();

		}
		public override async Task<RPC.Livro?> Get(IdLivro request, ServerCallContext context)
        {
            try{
                return await repLivros.GetAsync(request.Id);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
        }

        public override async Task<Empty> Remover(RPC.IdLivro request, ServerCallContext context)
        {
            try {
                await repLivros.RemoverAsync(request.Id);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();
        }

        public override async Task<ListaLivros> Buscar(ParamBusca request, ServerCallContext context)
        {
            
            try{
				IEnumerable<Livro> Livros = await repLivros.BuscarAsync(request.NomeLivro, request.NomeAutor, request.IdTags);
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
                await repLivros.EditarAsync(request);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();
        }
    }
}