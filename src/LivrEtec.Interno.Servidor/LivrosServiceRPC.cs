using Grpc.Core;
using LivrEtec.Interno;
using LivrEtec.Interno.RPC;

namespace LivrEtec.Interno.Servidor
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

        public override Task<Empty> RegistrarLivro( RPCLivro request, ServerCallContext context)
        {
            if(request is not null)
                _acervoService.Livros.Registrar(request!);
            return Task.FromResult(new Empty());
        }
        public override Task<RPCLivro> Get(RPCIdLivro request, ServerCallContext context)
        {
            return Task.FromResult((RPCLivro)_acervoService.Livros.Get(request.Id)!);
        }
    }
}