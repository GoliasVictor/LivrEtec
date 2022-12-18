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
        public Exception GerarRpcException(Exception ex){
            var metadata = new Metadata(){
                { "Excecao" , ex.GetType().Name },
                { "Mensagem", ex.Message }
            };
            switch (ex)
            {
                case InvalidDataException InvalidData:
                    return new RpcException(new Status(StatusCode.InvalidArgument, $"Dados invalidos", ex), metadata);
                case ArgumentNullException ArgumentNull:
                    metadata.Add(nameof(ArgumentNull.ParamName),  ArgumentNull.Message);
                    return new RpcException(new Status(StatusCode.InvalidArgument, $"{ArgumentNull.ParamName} é nulo", ex), metadata);
                case InvalidOperationException InvalidOperation:
                    return new RpcException(new Status(StatusCode.InvalidArgument, "Operação Invalida"), metadata);
                default:
                    return new RpcException(new Status(StatusCode.Internal, "Erro interno", ex));
            }
        
        }

        public override async Task<Empty> Registrar(RPC.Livro request, ServerCallContext context)
		{
            try {
			    await _acervoService.Livros.RegistrarAsync(request!);
            }
            catch (Exception ex) {
                throw GerarRpcException(ex);
            }
            return new Empty();

		}
		public override async Task<RPC.Livro> Get(IdLivro request, ServerCallContext context)
        {
            try{
                return await _acervoService.Livros.GetAsync(request.Id) ?? null!;
            }
            catch (Exception ex) {
                throw GerarRpcException(ex);
            }
        }

        public override Task<Empty> Remover(RPC.IdLivro request, ServerCallContext context)
        {
            try {
                _acervoService.Livros.RemoverAsync(request.Id);
            }
            catch (Exception ex) {
                throw GerarRpcException(ex);
            }
            return Task.FromResult(new Empty());
        }

        public override async Task<EnumLivros> Buscar(ParamBusca request, ServerCallContext context)
        {
            var Tags = request.Tags.Select((t) => (Tag)t);
            try{
                return new EnumLivros() { 
                    Livros = { 
                        await _acervoService.Livros.BuscarAsync(request.NomeLivro, request.NomeAutor, Tags).Select(l=> (RPC.Livro)l).ToArrayAsync() 
                    }
                };
            }
            catch (Exception ex) {
                throw GerarRpcException(ex);
            }
        }

        public override async Task<Empty> Editar(RPC.Livro request, ServerCallContext context)
        {
            try{
                await _acervoService.Livros.EditarAsync(request!);
            }
            catch (Exception ex) {
                throw GerarRpcException(ex);
            }
            return new Empty();
        }
    }
}