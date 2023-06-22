using Grpc.Net.Client;
using LivrEtec.GIB.Services;
using Xunit.Abstractions;

namespace LivrEtec.Testes.TestesFinais;

[Trait("Category", "Remoto")]
public sealed class TestesTagsServiceRPC : TestesTagsService<TagsServiceRPC>
{
    protected override TagsServiceRPC tagsService { get; init; }
    public TestesTagsServiceRPC(ITestOutputHelper output)
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
            Cargo = cargoTeste
        };
        BDU.Cargos = new[] { UsuarioTeste.Cargo };
        foreach (Permissao perm in Permissoes.TodasPermissoes)
        {
            perm.Cargos = new List<Cargo>();
        }

        BDU.Usuarios = new[] { UsuarioTeste };
        BDU.SalvarDados();
        GrpcChannel channel = gRPCUtil.GetGrpChannel(Configuracao.UrlGIBAPI, UsuarioTeste);
        tagsService = new TagsServiceRPC(new GIB.RPC.Tags.TagsClient(channel), output.ToLogger<TagsServiceRPC>());
    }
}