using LivrEtec.GIB;
using Xunit.Abstractions;
using Grpc.Net.Client;

namespace LivrEtec.Testes.Remoto;

[Trait("Category", "Remoto")]
public sealed class TestesLivrosRPC: TestesLivro<RepLivroRPC>
{
	protected override RepLivroRPC RepLivros {get;init;}
    public TestesLivrosRPC(ConfiguradorTestes configurador, ITestOutputHelper output) 
		: base( 
			configurador, 
			output,
			new BDUtilMySQl(configurador.Config.StrConexaoMySQL, configurador.CreateLoggerFactory(output))
		)
	{
		GrpcChannel channel = gRPCUtil.GetGrpChannel(configurador.Config.UrlGIBAPI);
        RepLivros = new RepLivroRPC(new GIB.RPC.Livros.LivrosClient(channel),output.ToLogger<RepLivroRPC>());
    }
}