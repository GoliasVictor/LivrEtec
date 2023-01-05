using LivrEtec.GIB;
using RPC = LivrEtec.GIB.RPC;
using Microsoft.Extensions.Logging;
using LivrEtec.GIB.RPC;
using Grpc.Core;

namespace LivrEtec.GIB
{
    public sealed class RepLivroRPC:  IRepLivros
    {
        private readonly ILogger<RepLivroRPC> _logger;
        public readonly RPC::Livros.LivrosClient LivrosClientRPC;
        public RepLivroRPC(ILogger<RepLivroRPC> logger, RPC::Livros.LivrosClient livrosClientRPC)
        {
            LivrosClientRPC = livrosClientRPC;
            _logger = logger;
        }

        public async Task EditarAsync(Livro livro)
        {
            _ = livro ?? throw new ArgumentNullException(nameof(livro));
            if(livro.Tags.Any((t)=> t is null))
                throw new InvalidDataException("tag nula");

            livro.Tags ??= new();
            try{
                await LivrosClientRPC.EditarAsync(livro);
            }
            catch(RpcException ex){
               throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

        public async Task<Livro?> GetAsync(int id)
        {
            try{
                return await LivrosClientRPC.GetAsync(new IdLivro() { Id = id });
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex); 
            }
        }

        public async Task RegistrarAsync(Livro livro)
        { 
            if(livro is not null){
                livro.Tags ??= new();
                livro.Autores ??= new();    
            }
            Validador.ErroSeInvalido(livro);
            RPC::Livro livroRPC = livro;
            if (string.IsNullOrWhiteSpace(livro.Nome) || livro.Id < 0)
                throw new InvalidDataException();
            try{
                await LivrosClientRPC.RegistrarAsync(livro);
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }

		}


		public async Task RemoverAsync(int id)
        {
            try{
                await LivrosClientRPC.RemoverAsync(new IdLivro(){ Id = id});
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

        public async Task<IEnumerable<Livro>> BuscarAsync(string nome, string nomeAutor, IEnumerable<int>? idTags)
        {
            nome ??= "";
            nomeAutor ??= "";
            idTags ??= new List<int>();
            try{
			    ListaLivros listaLivros = await LivrosClientRPC.BuscarAsync(new ParamBusca() { NomeLivro = nome, NomeAutor = nomeAutor, IdTags = { idTags }});
			    return listaLivros.Livros.Select(l=> (Livro)l!);
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

	}
}