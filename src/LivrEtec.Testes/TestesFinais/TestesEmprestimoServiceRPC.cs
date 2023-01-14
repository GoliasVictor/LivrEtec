using Grpc.Net.Client;
using LivrEtec.GIB;
using Xunit.Abstractions;
namespace LivrEtec.Testes.Remoto;

[Trait("Category", "Remoto")]
public sealed class TestesEmprestimoServiceRPC : TestesEmprestimoService<EmprestimoServiceRPC>
{
	protected override EmprestimoServiceRPC emprestimoService { get; init; }
	public TestesEmprestimoServiceRPC(ITestOutputHelper output)
		: base(
			output,
			new RelogioSistema(),
			new BDUtilMySQl(Configuracao.StrConexaoMySQL,LogUtils.CreateLoggerFactory(output))
		)
	{
		GrpcChannel channel = gRPCUtil.GetGrpChannel(Configuracao.UrlGIBAPI, usuarioTeste);
		
		var identidadeService = new IdentidadePermitidaStub(usuarioTeste);
		emprestimoService = new EmprestimoServiceRPC(
			LogUtils.CreateLogger<EmprestimoServiceRPC>(output),
			new GIB.RPC.Emprestimos.EmprestimosClient(channel)
		);
	}
}
