using LivrEtec.GIB;
using RPC = LivrEtec.GIB.RPC;
using Microsoft.Extensions.Logging;
using LivrEtec.GIB.RPC;

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

        public void Editar(Livro livro)
        {
            _ = livro ?? throw new ArgumentNullException(nameof(livro));
            livro.Tags ??= new List<Tag>();
            LivrosClientRPC.Editar(livro);
        }

        public Livro? Get(int id)
        {
            return LivrosClientRPC.Get(new IdLivro() { Id = id });
        }

        public void Registrar(Livro livro)
        {
            _ = livro ?? throw new ArgumentNullException(nameof(livro));
            if (string.IsNullOrWhiteSpace(livro.Nome) || livro.Id < 0)
                throw new InvalidDataException();
            LivrosClientRPC.Registrar(livro);
        }

        public void Remover(Livro livro)
        {
            _ = livro ?? throw new ArgumentNullException(nameof(livro));
            LivrosClientRPC.Remover(livro);
        }

        public IEnumerable<Livro> Buscar(string nome, string nomeAutor, IEnumerable<Tag>? tags)
        {
            nome ??= "";
            nomeAutor ??= "";
            tags ??= new List<Tag>();
            return LivrosClientRPC.Buscar(new ParamBusca() { NomeLivro = nome, NomeAutor = nomeAutor, Tags = { tags?.Select(t=> (RPC::Tag)t) } }).Livros.Select((l)=> (Livro)l!);
        }
    }
}