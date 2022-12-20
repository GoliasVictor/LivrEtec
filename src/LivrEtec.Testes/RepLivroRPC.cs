using LivrEtec.GIB;
using RPC = LivrEtec.GIB.RPC;
using Microsoft.Extensions.Logging;
using LivrEtec.GIB.RPC;
using Grpc.Core;

namespace LivrEtec.Testes
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


		private static Exception CriarExcecao(RpcException ex)
		{
			var Excecao = ex.Trailers.FirstOrDefault(p => p.Key == "excecao")?.Value;
			var Mensagem = ex.Trailers.FirstOrDefault(p => p.Key == "mensagem")?.Value;
            
			switch (Excecao)
			{
				case nameof(ArgumentNullException):
					var NomeParametro = ex.Trailers.FirstOrDefault(p => p.Key == "NomeParametro")?.Value;
					return new ArgumentNullException( Mensagem, ex );
                case nameof(InvalidDataException):
                    return new InvalidDataException(Mensagem, ex);
                case nameof(InvalidOperationException):
                    return new InvalidOperationException(Mensagem, ex);
                default:
                    return ex;
			}
		}

        public async Task EditarAsync(Livro livro)
        {
            _ = livro ?? throw new ArgumentNullException(nameof(livro));
            if(livro.Tags.Any((t)=> t is null))
                throw new ArgumentNullException();
            livro.Tags ??= new();
            try{
                await LivrosClientRPC.EditarAsync(livro);
            }
            catch(RpcException ex){
               throw CriarExcecao(ex);
            }
        }

        public async Task<Livro?> GetAsync(int id)
        {
            try{
                return await LivrosClientRPC.GetAsync(new IdLivro() { Id = id });
            }
            catch(RpcException ex){
                throw CriarExcecao(ex); 
            }
        }

        public async Task RegistrarAsync(Livro livro)
        {
            _ = livro ?? throw new ArgumentNullException(nameof(livro));
            livro.Tags ??= new();
            livro.Autores ??= new();
            livro.Descricao = "";
            if (string.IsNullOrWhiteSpace(livro.Nome) || livro.Id < 0)
                throw new InvalidDataException();
            try{
                await LivrosClientRPC.RegistrarAsync(livro);
            }
            catch(RpcException ex){
                throw CriarExcecao(ex);
            }

		}


		public async Task RemoverAsync(int id)
        {
            try{
                await LivrosClientRPC.RemoverAsync(new IdLivro(){ Id = id});
            }
            catch(RpcException ex){
                throw CriarExcecao(ex);
            }
        }

        public async Task<IEnumerable<Livro>> BuscarAsync(string nome, string nomeAutor, IEnumerable<Tag>? tags)
        {
            nome ??= "";
            nomeAutor ??= "";
            tags ??= new List<Tag>();
            EnumLivros enumLivros = default!;
            try{
			    enumLivros = await LivrosClientRPC.BuscarAsync(new ParamBusca() { NomeLivro = nome, NomeAutor = nomeAutor, Tags = { tags?.Select(t => (RPC::Tag)t) } });
            }
            catch(RpcException ex){
                throw CriarExcecao(ex);
            }
			return enumLivros.Livros.Select(l=> (Livro)l!);
        }

	}
}