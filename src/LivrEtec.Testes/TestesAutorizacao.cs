
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes;
[Collection("UsaBancoDeDados")]
public class TestesAutorizacao  : IClassFixture<ConfiguradorTestes>, IDisposable
{
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
			new Cargo(1, "Administrador", Permissoes.TodasPermissoes.ToList()),
			new Cargo(2, "Aluno", new (){
				Permissoes.Livro.Criar,
				Permissoes.Livro.Visualizar,
			}),
			new Cargo(3, "Aluno Estagiario", new (){
				Permissoes.Livro.Criar,
				Permissoes.Livro.Visualizar,
				Permissoes.Livro.Editar,
				Permissoes.Livro.Excluir,
				Permissoes.Emprestimo.Criar,
				Permissoes.Emprestimo.Excluir,
			})
		};
		Cargos = new[] { new Cargo(1,"oi", new() { Permissoes.Livro.Visualizar}) };
		Usuarios =  new []{
			new Usuario(1, "", "tavares", "Tavares", gCargo(1)),
			new Usuario(2, "", "Ze", "Zé", gCargo(1)),
			new Usuario(3, "", "Atlas", "Atlas", gCargo(1)),
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
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public void EhAutorizado_Autorizado(int idUsuario)
	{
		var usuario = gUsuario(idUsuario);
		var permissao =  Permissoes.Livro.Visualizar;

		var autorizado = AutorizacaoService.EhAutorizado(usuario, permissao);

		Assert.True(autorizado);
	}
	[Theory]
	[InlineData(2)]
	[InlineData(3)]
	public void EhAutorizado_NaoAutorizado(int idUsuario)
	{
		var usuario = gUsuario(idUsuario);
		var permissao =  Permissoes.Cargo.Criar;

		var autorizado = AutorizacaoService.EhAutorizado(usuario, permissao);
		
		Assert.False(autorizado);
	}


	public void Dispose()
	{
		BD.Dispose();
	}
}