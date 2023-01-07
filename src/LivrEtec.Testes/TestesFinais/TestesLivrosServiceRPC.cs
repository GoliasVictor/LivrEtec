using LivrEtec.GIB;
using Xunit.Abstractions;
using Grpc.Net.Client;

namespace LivrEtec.Testes.Remoto;

[Trait("Category", "Remoto")]
public sealed class TestesLivrosServiceRPC: TestesLivrosService<LivrosServiceRPC>
{
	protected override LivrosServiceRPC livrosService { get;init;}
    public TestesLivrosServiceRPC(ITestOutputHelper output) 
		: base (  
			output,
			new BDUtilMySQl(Configuracao.StrConexaoMySQL, LogUtils.CreateLoggerFactory(output))
		)
	{
		GrpcChannel channel = gRPCUtil.GetGrpChannel(Configuracao.UrlGIBAPI);
        livrosService = new LivrosServiceRPC(new GIB.RPC.Livros.LivrosClient(channel),output.ToLogger<LivrosServiceRPC>());
    }
}