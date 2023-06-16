using LivrEtec.Exceptions;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;
using Xunit.Abstractions;

namespace LivrEtec.Testes;
[Collection("UsaBancoDeDados")]
[Trait("Category", "Local")]
public class TestesIdentidade : IDisposable
{
    private readonly BDUtil BDU;
    private readonly IIdentidadeService Identidade;
    private const int IdCargoAdmin = 1;
    private const int IdCargoAnonimo = 2;
    private const int IdAdmin = 1;
    private const string LoginAdmin = "admin";
    private const int IdAnonimo = 2;
    private readonly (int Id, string Senha, string Hash)[] Senhas;

    private string gLogin(int id)
    {
        return BDU.Usuarios.First((s) => s.Id == id).Login;
    }

    private string gSenha(int id)
    {
        return Senhas.First((s) => s.Id == id).Senha;
    }

    private string gHash(int id)
    {
        return Senhas.First((s) => s.Id == id).Hash;
    }

    private static void AssertEhIgual<K>(IEnumerable<K> A, IEnumerable<K> B)
        where K : IComparable<K>
    {
        Assert.Equal(A.OrderBy(a => a), B.OrderBy(b => b));
    }
    public TestesIdentidade(ITestOutputHelper output)
    {
        BDU = new BDUtilSqlLite(LogUtils.CreateLoggerFactory(output));
        foreach (Permissao perm in Permissoes.TodasPermissoes)
        {
            perm.Cargos = new List<Cargo>();
        }

        Senhas = new[]{
            (IdAdmin, "admin"      , "e00cf25ad42683b3df678c61f42c6bda"),
            (2, "2@oCP06io1#q"  , "97a290347762986f757e7fe694b43e45")
        };
        BDU.Cargos = new[]{
            new Cargo(IdCargoAdmin, "Administrador", Permissoes.TodasPermissoes.ToList()),
            new Cargo(IdCargoAnonimo, "Anonimo", new (){}),
        };
        BDU.Usuarios = new[]{
            new Usuario(IdAdmin  , gHash(IdAdmin  ), LoginAdmin  , "Tavares" , BDU.gCargo(IdCargoAdmin)),
            new Usuario(IdAnonimo, gHash(IdAnonimo), "Atlas"  , "Atlas"   , BDU.gCargo(IdCargoAnonimo)),
        };
        BDU.SalvarDados();
        PacaContext BD = BDU.CriarContexto();
        ILoggerFactory loggerFactory = LogUtils.CreateLoggerFactory(output);
        var repUsuarios = new RepUsuarios(BD, loggerFactory.CreateLogger<RepUsuarios>());
        Identidade = new IdentidadeService(
            repUsuarios,
            new AutorizacaoService(repUsuarios, loggerFactory.CreateLogger<AutorizacaoService>()),
            new AutenticacaoService(repUsuarios, loggerFactory.CreateLogger<AutenticacaoService>()),
            loggerFactory.CreateLogger<IdentidadeService>()
        );
    }
     
 
    void AssertUsuarioIgual(Usuario Esperado, Usuario Atual)
    {
        if (Esperado is null || Atual is null)
        {
            Assert.False(Esperado == Atual);
            return;
        }
        Assert.Equal(Esperado.Nome, Atual.Nome);
        Assert.Equal(Esperado.Id, Atual.Id);
        Assert.Equal(Esperado.Senha, Atual.Senha);
        Assert.Equal(Esperado.Login, Atual.Login);
        AssertEhIgual(Esperado.Cargo.Permissoes, Atual.Cargo.Permissoes);
    }
    [Fact]
    public async Task Login_SenhaValidaAsync()
    {
        var idUsuario = IdAdmin;
        var login = LoginAdmin;

        await Identidade.Login(login, gHash(idUsuario), true);
        Usuario Usuario = Identidade.Usuario!;

        Assert.True(Identidade.EstaAutenticado);
        Assert.NotNull(Usuario);
        Assert.Equal(Usuario.Id, idUsuario);
        Assert.Equal(Usuario.Login, login);
        Assert.Null(Usuario.Senha);
    }

    [Theory]
    [InlineData("admin1")]
    [InlineData("root")]
    public async Task Login_SenhaInvalidaAsync(string senha)
    {
        var login = LoginAdmin;

        await Identidade.Login(login, senha, false);

        Assert.False(Identidade.EstaAutenticado);
        Assert.NotEqual(Identidade.Usuario, BDU.gUsuario(IdAdmin));
    }
    [Theory]

    [InlineData(LoginAdmin, null)]
    [InlineData(null, "senha")]
    public async Task Login_ArgumentosNulosAsync(string login, string senha)
    {
        _ = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await Identidade.Login(login, senha, true);
        });
    }
    
    [Fact]
    public async Task Login_UsuarioInexistente()
    {
        var login = "usuarioinexistente";
        var senha = "senha";
        await Identidade.Login(login, senha, false);

        Assert.False(Identidade.EstaAutenticado);
        Assert.NotEqual(Identidade.Usuario?.Id,IdAdmin);
    }

    [Theory]
    [InlineData(IdAdmin)]
    [InlineData(IdAnonimo)]
    public async Task CarregarUsuario_UsuarioValidoAsync(int idUsuario)
    {
        var usuario = BDU.gUsuario(idUsuario);
        Identidade.Usuario = new Usuario() { Id = idUsuario };
        Identidade.EstaAutenticado = true;

        await Identidade.CarregarUsuario();

        AssertUsuarioIgual(Identidade.Usuario, usuario);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(-2)]
    public async Task CarregarUsuario_UsuarioInvalidoAsync(int idUsuario)
    {
        Identidade.Usuario = new Usuario() { Id = idUsuario };
        Identidade.EstaAutenticado = true;

        _ = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await Identidade.CarregarUsuario();
        });
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1)]
    public async Task CarregarUsuario_NaoAutenticadoAsync(int idUsuario)
    {
        Identidade.Usuario = new Usuario() { Id = idUsuario };
        Identidade.EstaAutenticado = false;

        _ = await Assert.ThrowsAsync<NaoAutenticadoException>(async () =>
        {
            await Identidade.CarregarUsuario();
        });
    }

    [Fact]
    public async Task CarregarUsuario_UsuarioNulo()
    {
        Identidade.Usuario = null;
        Identidade.EstaAutenticado = true;

        _ = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await Identidade.CarregarUsuario();
        });
    }

    [Theory]
    [InlineData(IdAdmin, true)]
    [InlineData(IdAnonimo, false)]
    public async Task EhAutorizadoAsync(int idUsuario, bool ExpectativaAutorizado)
    {
        Permissao permissao = Permissoes.Cargo.Criar;
        Identidade.Usuario = BDU.gUsuario(idUsuario);
        Identidade.EstaAutenticado = true;

        var Autorizado = await Identidade.EhAutorizado(permissao);

        Assert.Equal(Autorizado, ExpectativaAutorizado);
    }

    [Fact]
    public async Task EhAutorizado_NaoAutenticadoAsync()
    {
        Permissao permissao = Permissoes.Cargo.Criar;
        Identidade.Usuario = new Usuario() { Id = IdAdmin };

        var Autorizado = await Identidade.EhAutorizado(permissao);
        Assert.False(Autorizado);
    }


    [Fact]
    public async Task EhAutorizado_UsuarioNulo()
    {
        Permissao permissao = Permissoes.Cargo.Criar;
        Identidade.Usuario = null;

        var autorizado = await Identidade.EhAutorizado(permissao);

        Assert.False(autorizado);
    }
    [Fact]
    public async Task ErroSeNaoAutorizado_SemErro()
    {
        var idUsuario = IdAdmin;
        Permissao permissao = Permissoes.Cargo.Criar;
        Identidade.Usuario = BDU.gUsuario(idUsuario);
        Identidade.EstaAutenticado = true;

        await Identidade.ErroSeNaoAutorizado(permissao);
    }

    [Fact]
    public async Task ErroSeNaoAutorizado_NaoAutorizadoAsync()
    {
        Permissao permissao = Permissoes.Cargo.Criar;
        Identidade.Usuario = new Usuario() { Id = IdAnonimo };
        Identidade.EstaAutenticado = true;

        var Autorizado = await Identidade.EhAutorizado(permissao);

        _ = await Assert.ThrowsAsync<NaoAutorizadoException>(async () =>
        {
            await Identidade.ErroSeNaoAutorizado(permissao);
        });
    }

    [Fact]
    public async Task ErroSeNaoAutorizado_NaoAutenticadoAsync()
    {
        Permissao permissao = Permissoes.Cargo.Criar;
        Identidade.Usuario = new Usuario() { Id = IdAdmin };
        Identidade.EstaAutenticado = false;

        var Autorizado = await Identidade.EhAutorizado(permissao);

        _ = await Assert.ThrowsAsync<NaoAutenticadoException>(async () =>
        {
            await Identidade.ErroSeNaoAutorizado(permissao);
        });
    }

    [Fact]
    public async Task ErroSeNaoAutorizado_UsuarioNulo()
    {
        Permissao permissao = Permissoes.Cargo.Criar;
        Identidade.Usuario = null;

        _ = await Assert.ThrowsAsync<NaoAutenticadoException>(async () =>
        {
            await Identidade.ErroSeNaoAutorizado(permissao);
        });
    }

    public void Dispose()
    {
        BDU.Dispose();
    }
}
