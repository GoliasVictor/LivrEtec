
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
        var httpClient = new HttpClient();
		httpClient.DefaultRequestHeaders.Add("id",(1).ToString());
		var grpcChannelOptions  = new GrpcChannelOptions(){
			Credentials = Grpc.Core.ChannelCredentials.Insecure,
			HttpClient = httpClient
		};
	
        var channel = GrpcChannel.ForAddress(configurador.Config.UrlGIBAPI, grpcChannelOptions);
		
        repLivrosRPC = new RepLivroRPC(output.ToLogger<RepLivroRPC>(),new GIB.RPC.Livros.LivrosClient(channel));
    }
}