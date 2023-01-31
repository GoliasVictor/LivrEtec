using Grpc.Net.Client;
using LivrEtec.GIB.Services;
using Xunit.Abstractions;
namespace LivrEtec.Testes.TestesFinais;

[Trait("Category", "Remoto")]
public sealed class TestesEmprestimoServiceRPC : TestesEmprestimoService<EmprestimoServiceRPC>
{
    protected override EmprestimoServiceRPC emprestimoService { get; init; }
    public TestesEmprestimoServiceRPC(ITestOutputHelper output)
        : base(
            output,
            new RelogioSistema(),
            new BDUtilMySQl(
                Configuracao.StrConexaoMySQL ?? throw new Exception("Defina uma string de conexão do MySQL"), 
                LogUtils.CreateLoggerFactory(output)
            )
        )
    {
        GrpcChannel channel = gRPCUtil.GetGrpChannel(Configuracao.UrlGIBAPI, usuarioTeste);
        _ = new IdentidadePermitidaStub(usuarioTeste);
        emprestimoService = new EmprestimoServiceRPC(
            LogUtils.CreateLogger<EmprestimoServiceRPC>(output),
            new GIB.RPC.Emprestimos.EmprestimosClient(channel)
        );
    }
}
