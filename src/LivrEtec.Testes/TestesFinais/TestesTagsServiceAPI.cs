using LivrEtec.Testes.Client;
using Xunit.Abstractions;

namespace LivrEtec.Testes.TestesFinais;

[Trait("Category", "Remoto")]
public sealed class TestesTagsServiceAPI : TestesTagsService<TagsServiceAPI>
{
    protected override TagsServiceAPI tagsService { get; init; }
    public TestesTagsServiceAPI(ITestOutputHelper output)
        : base(
            output,
            new BDUtilMySQl(
                Configuracao.StrConexaoMySQL ?? throw new Exception("Defina uma string de conexão do MySQL"),
                LogUtils.CreateLoggerFactory(output)
            )
        )
    {
        var cargoTeste = new Cargo()
        {
            Id = 10,
            Nome = "Cargo Teste",
            Permissoes = Permissoes.TodasPermissoes.ToList()
        };
        var UsuarioTeste = new Usuario()
        {
            Id = 100,
            Nome = "Usuario Teste",
            Login = "teste",
            Senha = "senha",
            Cargo = cargoTeste
        };
        BDU.Cargos = new[] { UsuarioTeste.Cargo };
        foreach (Permissao perm in Permissoes.TodasPermissoes)
        {
            perm.Cargos = new List<Cargo>();
        }

        BDU.Usuarios = new[] { UsuarioTeste };
        BDU.SalvarDados();
        HttpClient client = HttpUtils.GetHttpClient(Configuracao.UrlGIBAPI, UsuarioTeste);
        tagsService = new TagsServiceAPI(client, output.ToLogger<TagsServiceAPI>());
    }
}