using Grpc.Net.Client;
using LivrEtec.GIB;
using Xunit.Abstractions;
namespace LivrEtec.Testes.Remoto;

[Trait("Category", "Remoto")]
public sealed class TestesEmprestimoServiceRPC : TestesEmprestimoService<EmprestimoServiceRPC>
{
	protected override EmprestimoServiceRPC emprestimoService { get; init; }
	public TestesEmprestimoServiceRPC(ConfiguradorTestes configurador, ITestOutputHelper output)
		: base(
			configurador,
			output,
			new RelogioSistema(),
			new BDUtilMySQl(configurador.Config.StrConexaoMySQL, configurador.CreateLoggerFactory(output))
		)
	{
		GrpcChannel channel = gRPCUtil.GetGrpChannel(configurador.Config.UrlGIBAPI);
		
		var identidadeService = new IdentidadePermitidaStub(usuarioTeste);
		emprestimoService = new EmprestimoServiceRPC(
			configurador.CreateLogger<EmprestimoServiceRPC>(output),
			new GIB.RPC.Emprestimos.EmprestimosClient(channel)
		);
	}
}
