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

        public async Task EditarAsync(Livro livro)
        {
            _ = livro ?? throw new ArgumentNullException(nameof(livro));
            livro.Tags ??= new List<Tag>();
            await LivrosClientRPC.EditarAsync(livro);
        }

        public async Task<Livro?> GetAsync(int id)
        {
            return await LivrosClientRPC.GetAsync(new IdLivro() { Id = id });
        }

        public async Task RegistrarAsync(Livro livro)
        {
            _ = livro ?? throw new ArgumentNullException(nameof(livro));
            if (string.IsNullOrWhiteSpace(livro.Nome) || livro.Id < 0)
                throw new InvalidDataException();
            await LivrosClientRPC.RegistrarAsync(livro);
        }

        public async Task RemoverAsync(Livro livro)
        {
            _ = livro ?? throw new ArgumentNullException(nameof(livro));
            await LivrosClientRPC.RemoverAsync(livro);
        }

        public async IAsyncEnumerable<Livro> BuscarAsync(string nome, string nomeAutor, IEnumerable<Tag>? tags)
        {
            nome ??= "";
            nomeAutor ??= "";
            tags ??= new List<Tag>();
			EnumLivros enumLivros = await LivrosClientRPC.BuscarAsync(new ParamBusca() { NomeLivro = nome, NomeAutor = nomeAutor, Tags = { tags?.Select(t => (RPC::Tag)t) } });
			foreach (var livro in enumLivros.Livros)
                yield return (Livro)livro!;
        }

	}
}