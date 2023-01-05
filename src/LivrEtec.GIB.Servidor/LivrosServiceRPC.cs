using Grpc.Core;
using LivrEtec.GIB.RPC;

namespace LivrEtec.GIB.Servidor
{
    public sealed class LivroServiceRPC : Livros.LivrosBase
    {
        private readonly ILogger<LivroServiceRPC> _logger;
        private readonly IAcervoService _acervoService;
        public LivroServiceRPC(ILogger<LivroServiceRPC> logger, IAcervoService acervoService)
        {
            _logger = logger;
            _acervoService = acervoService;
        }


        public override async Task<Empty> Registrar(RPC.Livro request, ServerCallContext context)
		{
            try {
			    await _acervoService.Livros.RegistrarAsync(request);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();

		}
		public override async Task<RPC.Livro?> Get(IdLivro request, ServerCallContext context)
        {
            try{
                return await _acervoService.Livros.GetAsync(request.Id);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
        }

        public override async Task<Empty> Remover(RPC.IdLivro request, ServerCallContext context)
        {
            try {
                await _acervoService.Livros.RemoverAsync(request.Id);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();
        }

        public override async Task<ListaLivros> Buscar(ParamBusca request, ServerCallContext context)
        {
            
            try{
				IEnumerable<Livro> Livros = await _acervoService.Livros.BuscarAsync(request.NomeLivro, request.NomeAutor, request.IdTags);
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
                await _acervoService.Livros.EditarAsync(request);
            }
            catch (Exception ex) {
                throw ManipuladorException.ExceptionToRpcException(ex);
            }
            return new Empty();
        }
    }
}