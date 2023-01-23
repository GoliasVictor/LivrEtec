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

    private string gSenha(int id)
    {
        return Senhas.First((s) => s.Id == id).Senha;
    }

    private string gHash(int id)
    {
        return Senhas.First((h) => h.Id == id).Hash;
    }

    public TestesAutenticacao(ITestOutputHelper output)
    {
        Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = LogUtils.CreateLoggerFactory(output);
        BDU = new BDUtilSqlLite(loggerFactory);
        var Cargo = new Cargo(1, "cargo", new List<Permissao>());
        Senhas = new[]{
            (1, "Senha"         ,"be6b9084a5dcdb09af8f433557a2119c"),
            (2, "Senha"         , "14621de3463eb7e1b3606d5514bbf800"),
            (3, "2@oCP06io1#q"  , "7b3608972fed79f056fe915e725f536e")
        };
        BDU.Usuarios = new[]{
            new Usuario(1, gHash(1), "tavares", "Tavares"   , Cargo),
            new Usuario(2, gHash(2), "Atlas"    , "Atlas"   , Cargo),
            new Usuario(3, gHash(3), "Atlas"    , "Atlas"   , Cargo),
        };
        BDU.SalvarDados();
        BD = BDU.CriarContexto();

        var repUsuarios = new RepUsuarios(BD, LogUtils.CreateLogger<RepUsuarios>(output));
        autenticacaoService = new AutenticacaoService(
            repUsuarios,
            LogUtils.CreateLogger<AutenticacaoService>(output));
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void EhAutentico_Autentico(int idUsuario)
    {
        var senha = gSenha(idUsuario);

        var autentico = await autenticacaoService.EhAutentico(idUsuario, AutenticacaoService.GerarHahSenha(idUsuario, senha));

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

    [Fact]

    public async Task EhAutentico_UsuarioInvalidoAsync()
    {
        var senha = "Qualquer senha aleatoria errada";
        var idUsuario = -10;

        _ = await Assert.ThrowsAsync<ArgumentException>(async () =>
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