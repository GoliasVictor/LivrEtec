using Grpc.Net.Client;
using LivrEtec.GIB.Services;
using Xunit.Abstractions;

namespace LivrEtec.Testes.TestesFinais;

[Trait("Category", "Remoto")]
public sealed class TestesLivrosServiceRPC : TestesLivrosService<LivrosServiceRPC>
{
    protected override LivrosServiceRPC livrosService { get; init; }
    public TestesLivrosServiceRPC(ITestOutputHelper output)
        : base(
            output,
            new BDUtilMySQl(Configuracao.StrConexaoMySQL, LogUtils.CreateLoggerFactory(output))
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
        GrpcChannel channel = gRPCUtil.GetGrpChannel(Configuracao.UrlGIBAPI, UsuarioTeste);
        livrosService = new LivrosServiceRPC(new GIB.RPC.Livros.LivrosClient(channel), output.ToLogger<LivrosServiceRPC>());
    }
}