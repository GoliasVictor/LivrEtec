using Xunit.Abstractions;
using LivrEtec.GIB.Services.Cliente;
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
        HttpClient client = HttpUtils.GetHttpClient(Configuracao.UrlGIBAPI, usuarioTeste);
        _ = new IdentidadePermitidaStub(usuarioTeste);
        emprestimoService = new EmprestimoServiceRPC(
            LogUtils.CreateLogger<EmprestimoServiceRPC>(output),
            client
        );
    }
}
