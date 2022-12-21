
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using LivrEtec.GIB;
namespace LivrEtec.Testes;

[Trait("Category", "Remoto")]
public class TestesLivrosRPC: TestesLivro<RepLivroRPC>
{
	RepLivroRPC repLivrosRPC;
	protected override RepLivroRPC RepLivros => repLivrosRPC;
    public TestesLivrosRPC(ConfiguradorTestes configurador) : base(configurador)
	{
        var channel = GrpcChannel.ForAddress(configurador.Config.UrlGIBAPI);
        repLivrosRPC = new RepLivroRPC(configurador.loggerFactory.CreateLogger<RepLivroRPC>(),new GIB.RPC.Livros.LivrosClient(channel));
    }
}