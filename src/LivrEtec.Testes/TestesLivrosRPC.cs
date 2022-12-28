
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using LivrEtec.GIB;
using Xunit.Abstractions;

namespace LivrEtec.Testes;

[Trait("Category", "Remoto")]
public class TestesLivrosRPC: TestesLivro<RepLivroRPC>
{
	RepLivroRPC repLivrosRPC;
	protected override RepLivroRPC RepLivros => repLivrosRPC;
    public TestesLivrosRPC(ConfiguradorTestes configurador, ITestOutputHelper output) : base( configurador, output)
	{
        
        var channel = GrpcChannel.ForAddress(configurador.Config.UrlGIBAPI);
        repLivrosRPC = new RepLivroRPC(output.ToLogger<RepLivroRPC>(),new GIB.RPC.Livros.LivrosClient(channel));
    }
}