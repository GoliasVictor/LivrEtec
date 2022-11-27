
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes;
[Collection("UsaBancoDeDados")]
public class TestesAutorizacao  : IClassFixture<ConfiguradorTestes>, IDisposable
{
	const int IdAdministrador = 1;
	const int IdAnonimo = 2;
	PacaContext BD;
	AutorizacaoService AutorizacaoService;
	Usuario[] Usuarios;
	Cargo[] Cargos;
	Usuario gUsuario(int id) => Usuarios.First((u)=> u.Id == id); 
	Cargo gCargo(int id) => Cargos.First((c)=> c.Id == id); 
	public static bool EnumerableIgual<T>( IEnumerable<T> A, IEnumerable<T> B){
		return Enumerable.SequenceEqual(A.OrderBy((a)=>a),B.OrderBy(b=>b));
	}
	public TestesAutorizacao(ConfiguradorTestes configurador)
	{ 	

		Cargos = new[]{
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
		Usuarios =  new []{
			new Usuario(1, "", "tavares", "Tavares", gCargo(IdAdministrador)),
			new Usuario(2, "", "Ze", "Zé", gCargo(IdAnonimo)),
			new Usuario(3, "", "Paca", "Paca", gCargo(3)),
			new Usuario(4, "", "Atlas", "Atlas", gCargo(4)),
		};
		foreach (var perm in Permissoes.TodasPermissoes)
			perm.Cargos = new List<Cargo>();
        BD = new PacaContext(configurador.Config,LoggerFactory.Create((lb)=> { 
			lb.AddConsole();
			lb.AddFilter((_,_, logLevel)=> logLevel >= LogLevel.Information);
		}));

		BD = new PacaContext(configurador.Config);
        BD.Database.EnsureDeleted();
        BD.Database.EnsureCreated();
		BD.Permissoes.AddRange(Permissoes.TodasPermissoes);
		BD.Cargos.AddRange(Cargos);
		BD.Usuarios.AddRange(Usuarios);
		BD.SaveChanges();
		AutorizacaoService =  new AutorizacaoService(BD, null!);
	}
	[Theory]
	[InlineData(IdAdministrador)]
	[InlineData(3)]
	[InlineData(4)]
	public void EhAutorizado_Autorizado(int idUsuario)
	{
		var usuario = gUsuario(idUsuario);
		var permissao =  Permissoes.Livro.Visualizar;

		var autorizado = AutorizacaoService.EhAutorizado(usuario, permissao);

		Assert.True(autorizado);
	}
	[Theory]
	[InlineData(IdAnonimo)]
	[InlineData(3)]
	[InlineData(4)]
	public void EhAutorizado_NaoAutorizado(int idUsuario)
	{
		var usuario = gUsuario(idUsuario);
		var permissao =  Permissoes.Cargo.Criar;

		var autorizado = AutorizacaoService.EhAutorizado(usuario, permissao);
		
		Assert.False(autorizado);
	}

    [Fact]
    public void ErroSeNaoAutorizado_Nada()
    {
        var usuario = gUsuario(IdAdministrador);
        var permissao = Permissoes.Cargo.Criar;
        AutorizacaoService.ErroSeNaoAutorizado(usuario, permissao);
    }
    [Fact]
    public void ErroSeNaoAutorizado_NaoAutorizadoException()
    {
        var usuario = gUsuario(IdAnonimo);
        var permissao = Permissoes.Cargo.Criar;
		Assert.Throws<NaoAutorizadoException>(() => {
			AutorizacaoService.ErroSeNaoAutorizado(usuario, permissao);
		});
    }

    public void Dispose()
	{
		BD.Dispose();
	}
}