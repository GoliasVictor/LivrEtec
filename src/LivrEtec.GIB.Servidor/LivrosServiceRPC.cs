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

        public override async Task<Empty> RegistrarLivro(RPC.Livro request, ServerCallContext context)
        {
            if(request is not null)
                await _acervoService.Livros.RegistrarAsync(request!);
            return new Empty();
        }
        public override async Task<RPC.Livro> Get(IdLivro request, ServerCallContext context)
        {
            return await _acervoService.Livros.GetAsync(request.Id) ?? null!;
        }
    }
}