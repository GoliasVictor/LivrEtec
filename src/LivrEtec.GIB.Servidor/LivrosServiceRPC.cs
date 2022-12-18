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
            if(request is not null)
                await _acervoService.Livros.RegistrarAsync(request!);
            return new Empty();
        }
        public override async Task<RPC.Livro> Get(IdLivro request, ServerCallContext context)
        {
            return await _acervoService.Livros.GetAsync(request.Id) ?? null!;
        }

        public override Task<Empty> Remover(RPC.Livro request, ServerCallContext context)
        {
            if (request is not null)
                _acervoService.Livros.RemoverAsync(request!);
            return Task.FromResult(new Empty());
        }

        public override async Task<EnumLivros> Buscar(ParamBusca request, ServerCallContext context)
        {
            var Tags = request.Tags.Select((t) => (Tag)t);
            return new EnumLivros() { 
                Livros = { 
                    await _acervoService.Livros.BuscarAsync(request.NomeLivro, request.NomeAutor, Tags).Select(l=> (RPC.Livro)l).ToArrayAsync() 
                }
            };
        }

        public override async Task<Empty> Editar(RPC.Livro request, ServerCallContext context)
        {

            if (request is not null)
                await _acervoService.Livros.EditarAsync(request!);
            return new Empty();
        }
    }
}