using Xunit.Abstractions;
using LivrEtec.Testes.APIClient;
namespace LivrEtec.Testes.TestesFinais;

[Trait("Category", "Remoto")]
public sealed class TestesEmprestimoServiceAPI : TestesEmprestimoService<EmprestimoServiceAPI>
{
    protected override EmprestimoServiceAPI emprestimoService { get; init; }
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
        emprestimoService = new EmprestimoServiceAPI(
            LogUtils.CreateLogger<EmprestimoServiceAPI>(output),
            client
        );
    }
}
