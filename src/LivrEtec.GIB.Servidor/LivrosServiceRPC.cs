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

        public override Task<Empty> Registrar(RPC.Livro request, ServerCallContext context)
        {
            if(request is not null)
                _acervoService.Livros.Registrar(request!);
            return Task.FromResult(new Empty());
        }
        public override Task<RPC.Livro> Get(IdLivro request, ServerCallContext context)
        {
            return Task.FromResult((RPC.Livro)_acervoService.Livros.Get(request.Id)!);
        }

        public override Task<Empty> Remover(RPC.Livro request, ServerCallContext context)
        {
            if (request is not null)
                _acervoService.Livros.Remover(request!);
            return Task.FromResult(new Empty());
        }

        public override Task<EnumLivros> Buscar(ParamBusca request, ServerCallContext context)
        {
            var Tags = request.Tags.Select((t) => (Tag)t);
            return Task.FromResult( new EnumLivros() { 
                Livros = { 
                    _acervoService.Livros.Buscar(request.NomeLivro, request.NomeAutor, Tags)
                                         .Select(l=> (RPC.Livro)l) 
                }
            });
        }

        public override Task<Empty> Editar(RPC.Livro request, ServerCallContext context)
        {

            if (request is not null)
                _acervoService.Livros.Editar(request!);
            return Task.FromResult(new Empty());
        }
    }
}