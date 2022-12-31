
using Grpc.Net.Client;
using LivrEtec.GIB;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LivrEtec.Testes;

[Trait("Category", "Remoto")]
public class TestesEmprestimoServiceRPC : TestesEmprestimoService<EmprestimoServiceRPC> 
{
	protected override EmprestimoServiceRPC emprestimoService {get; init; }
	public TestesEmprestimoServiceRPC(ConfiguradorTestes configurador, ITestOutputHelper output) : base(configurador, output, new RelogioSistema())
	{
		    string Endereco = configurador.Config.UrlGIBAPI 
            ?? throw new Exception("Endereço da api interna do GIB indefinido");
		var httpClient = new HttpClient();
		httpClient.DefaultRequestHeaders.Add("id", ID_USUARIO_TESTE.ToString());
		var grpcChannelOptions  = new GrpcChannelOptions(){
			Credentials = Grpc.Core.ChannelCredentials.Insecure,
			HttpClient = httpClient
		};
	
        var channel = GrpcChannel.ForAddress(configurador.Config.UrlGIBAPI, grpcChannelOptions);
		
		var identidadeService = new IdentidadePermitidaStub(usuarioTeste);
		emprestimoService = new EmprestimoServiceRPC(
			configurador.CreateLogger<EmprestimoServiceRPC>(output),
			new GIB.RPC.Emprestimos.EmprestimosClient(channel)
		);
	} 

}
