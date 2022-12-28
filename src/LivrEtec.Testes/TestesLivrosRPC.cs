
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
        string Endereco = configurador.Config.UrlGIBAPI 
            ?? throw new Exception("Endereço da api interna do GIB indefinido");
        var channel = GrpcChannel.ForAddress(configurador.Config.UrlGIBAPI);
        repLivrosRPC = new RepLivroRPC(output.ToLogger<RepLivroRPC>(),new GIB.RPC.Livros.LivrosClient(channel));
    }
}