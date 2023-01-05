using LivrEtec.GIB;
using Xunit.Abstractions;
using Grpc.Net.Client;

namespace LivrEtec.Testes.Remoto;

[Trait("Category", "Remoto")]
public sealed class TestesLivrosRPC: TestesLivro<RepLivroRPC>
{
	protected override RepLivroRPC RepLivros {get;init;}
    public TestesLivrosRPC(ITestOutputHelper output) 
		: base (  
			output,
			new BDUtilMySQl(Configuracao.StrConexaoMySQL, LogUtils.CreateLoggerFactory(output))
		)
	{
		GrpcChannel channel = gRPCUtil.GetGrpChannel(Configuracao.UrlGIBAPI);
        RepLivros = new RepLivroRPC(new GIB.RPC.Livros.LivrosClient(channel),output.ToLogger<RepLivroRPC>());
    }
}