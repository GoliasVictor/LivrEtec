using Xunit.Abstractions;
using Grpc.Net.Client;
using LivrEtec.Models;
using LivrEtec.Testes.Utilitarios;
using LivrEtec.GIB.Services;

namespace LivrEtec.Testes.TestesFinais;

[Trait("Category", "Remoto")]
public sealed class TestesTagsServiceRPC : TestesTagsService<TagsServiceRPC>
{
    protected override TagsServiceRPC tagsService { get; init; }
    public TestesTagsServiceRPC(ITestOutputHelper output)
        : base(
            output,
            new BDUtilMySQl(Configuracao.StrConexaoMySQL, LogUtils.CreateLoggerFactory(output))
        )
    {
        Cargo cargoTeste = new Cargo()
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
        foreach (var perm in Permissoes.TodasPermissoes)
            perm.Cargos = new List<Cargo>();
        BDU.Usuarios = new[] { UsuarioTeste };
        BDU.SalvarDados();
        GrpcChannel channel = gRPCUtil.GetGrpChannel(Configuracao.UrlGIBAPI, UsuarioTeste);
        tagsService = new TagsServiceRPC(new GIB.RPC.Tags.TagsClient(channel), output.ToLogger<TagsServiceRPC>());
    }
}