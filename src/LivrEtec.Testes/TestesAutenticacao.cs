using Xunit.Abstractions;

namespace LivrEtec.Testes;
[Collection("UsaBancoDeDados")]
[Trait("Category", "Local")]
public class TestesAutenticacao : IDisposable
{
    private readonly BDUtil BDU;
    private readonly PacaContext BD;
    private readonly IAutenticacaoService autenticacaoService;
    private readonly (int Id, string Senha, string Hash)[] Senhas;


    private string gHash(int id)
    {
        return BDU.Senhas.First((s) => s.IdUsuario == id).Hash;
    }

    public TestesAutenticacao(ITestOutputHelper output)
    {
        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = LogUtils.CreateLoggerFactory(output);
        BDU = new BDUtilSqlLite(loggerFactory);
        var Cargo = new Cargo(1, "cargo", new List<Permissao>());
        BDU.Senhas = new[]{
            new Senha(1, IAutenticacaoService.GerarHahSenha(1, "Senha")),
            new Senha(2, IAutenticacaoService.GerarHahSenha(2, "Senha")),
            new Senha(3, IAutenticacaoService.GerarHahSenha(3, "2@oCP06io1#q"))
        };

        BDU.Usuarios = new[]{
            new Usuario(1, "tavares", "Tavares"   , Cargo),
            new Usuario(2, "Atlas"    , "Atlas"   , Cargo),
            new Usuario(3, "Atlas"    , "Atlas2"   , Cargo),
        };
    
        BDU.SalvarDados();
        BD = BDU.CriarContexto();

        var repSenhas = new RepSenhas(BD, LogUtils.CreateLogger<RepSenhas>(output));
        autenticacaoService = new AutenticacaoService(
            repSenhas,
            LogUtils.CreateLogger<AutenticacaoService>(output));
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void EhAutentico_Autentico(int idUsuario)
    {
        var senha = gHash(idUsuario);

        var autentico = await autenticacaoService.EhAutentico(idUsuario, senha);

        Assert.True(autentico);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task EhAutentico_NaoAutenticoAsync(int idUsuario)
    {
        var senha = "Qualquer senha aleatoria errada";

        var autentico = await autenticacaoService.EhAutentico(idUsuario, senha);

        Assert.False(autentico);
    }

    [Theory]

    [InlineData(-1)]
    [InlineData(100)]
    public async Task EhAutentico_UsuarioInvalidoAsync(int idUsuario)
    {
        var senha = "Qualquer senha aleatoria errada";

        _ = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            _ = await autenticacaoService.EhAutentico(idUsuario, senha);
        });
    }

    [Fact]
    public async Task EhAutentico_SenhaNulaAsync()
    {
        string senha = null!;
        var idUsuario = 1;

        _ = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            _ = await autenticacaoService.EhAutentico(idUsuario, senha);
        });
    }

    public void Dispose()
    {
        BD.Dispose();
    }
}