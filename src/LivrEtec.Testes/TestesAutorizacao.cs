using LivrEtec.Exceptions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LivrEtec.Testes;
[Collection("UsaBancoDeDados")]
[Trait("Category", "Local")]
public class TestesAutorizacao : IDisposable
{
    private const int IdAdministrador = 1;
    private const int IdAnonimo = 2;
    private readonly BDUtil BDU;
    private readonly IAutorizacaoService AutorizacaoService;
    public TestesAutorizacao(ITestOutputHelper output)
    {

        ILoggerFactory loggerFactory = LogUtils.CreateLoggerFactory(output);
        BDU = new BDUtilSqlLite(loggerFactory);
        foreach (Permissao perm in Permissoes.TodasPermissoes)
        {
            perm.Cargos = new List<Cargo>();
        }

        BDU.BDPermissoes = Permissoes.TodasPermissoes;
        BDU.Cargos = new[]{
            new Cargo(IdAdministrador, "Administrador", Permissoes.TodasPermissoes.ToList()),
            new Cargo(IdAnonimo, "Anonimo", new (){}),
            new Cargo(3, "Aluno", new (){
                Permissoes.Livro.Criar,
                Permissoes.Livro.Visualizar,
            }),
            new Cargo(4, "Aluno Estagiario", new (){
                Permissoes.Livro.Criar,
                Permissoes.Livro.Visualizar,
                Permissoes.Livro.Editar,
                Permissoes.Livro.Excluir,
                Permissoes.Emprestimo.Criar,
                Permissoes.Emprestimo.Excluir,
            })
        };
        BDU.Usuarios = new[]{
            new Usuario(1, "", "tavares", "Tavares" , BDU.gCargo(IdAdministrador)),
            new Usuario(2, "", "Ze"     , "Zé"      , BDU.gCargo(IdAnonimo)),
            new Usuario(3, "", "Paca"   , "Paca"    , BDU.gCargo(3)),
            new Usuario(4, "", "Atlas"  , "Atlas"   , BDU.gCargo(4)),
        };
        BDU.SalvarDados();
        PacaContext BD = BDU.CriarContexto();
        var repUsuarios = new RepUsuarios(BD, loggerFactory.CreateLogger<RepUsuarios>());
        AutorizacaoService = new AutorizacaoService(repUsuarios, loggerFactory.CreateLogger<AutorizacaoService>());
    }
    [Theory]
    [InlineData(IdAdministrador)]
    [InlineData(3)]
    [InlineData(4)]
    public async void EhAutorizado_Autorizado(int idUsuario)
    {
        Usuario usuario = BDU.gUsuario(idUsuario);
        Permissao permissao = Permissoes.Livro.Visualizar;


        var autorizado = await AutorizacaoService.EhAutorizado(usuario, permissao);

        Assert.True(autorizado);
    }
    [Theory]
    [InlineData(IdAnonimo)]
    [InlineData(3)]
    [InlineData(4)]
    public async void EhAutorizado_NaoAutorizado(int idUsuario)
    {
        Usuario usuario = BDU.gUsuario(idUsuario);
        Permissao permissao = Permissoes.Cargo.Criar;

        var autorizado = await AutorizacaoService.EhAutorizado(usuario, permissao);

        Assert.False(autorizado);
    }

    [Fact]
    public async Task ErroSeNaoAutorizado_NadaAsync()
    {
        Usuario usuario = BDU.gUsuario(IdAdministrador);
        Permissao permissao = Permissoes.Cargo.Criar;
        await AutorizacaoService.ErroSeNaoAutorizado(usuario, permissao);
    }
    [Fact]
    public async Task ErroSeNaoAutorizado_NaoAutorizadoExceptionAsync()
    {
        Usuario usuario = BDU.gUsuario(IdAnonimo);
        Permissao permissao = Permissoes.Cargo.Criar;
        _ = await Assert.ThrowsAsync<NaoAutorizadoException>(async () =>
        {
            await AutorizacaoService.ErroSeNaoAutorizado(usuario, permissao);
        });
    }

    [Fact]
    public async Task ErroSeNaoAutorizado_PermissaoNulaAsync()
    {
        Usuario usuario = BDU.gUsuario(IdAnonimo);
        Permissao permissao = null!;
        _ = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await AutorizacaoService.ErroSeNaoAutorizado(usuario, permissao);
        });
    }
    [Fact]
    public async Task ErroSeNaoAutorizado_PermissaoInvalidaAsync()
    {
        Usuario usuario = BDU.gUsuario(IdAnonimo);
        const int IdInvalido = 100;
        var permissao = new Permissao() { Id = IdInvalido };
        _ = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await AutorizacaoService.ErroSeNaoAutorizado(usuario, permissao);
        });
    }

    public void Dispose()
    {
        BDU.Dispose();
    }
}