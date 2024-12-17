using Xunit.Abstractions;
using LivrEtec.GIB.Services.Cliente;
using EmprestimoService = LivrEtec.GIB.Services.Cliente.EmprestimoService;
namespace LivrEtec.Testes.TestesFinais;

[Trait("Category", "Remoto")]
public sealed class TestesEmprestimoServiceAPI : TestesEmprestimoService<EmprestimoService>
{
    protected override EmprestimoService emprestimoService { get; init; }
    public TestesEmprestimoServiceAPI(ITestOutputHelper output)
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
        emprestimoService = new EmprestimoService(
            LogUtils.CreateLogger<EmprestimoService>(output),
            client
        );
    }
}
