using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LivrEtec.Testes.Local;
[Collection("UsaBancoDeDados")]
[Trait("Category", "Local")]
public class TestesIdentidade :  IDisposable
{
	readonly BDUtil BDU;
	readonly IIdentidadeService Identidade;
	const int IdCargoAdmin = 1;
	const int IdCargoAnonimo = 2;
	const int IdAdmin = 1;
	const int IdAnonimo = 2; 
	(int Id, string Senha, string Hash)[] Senhas;
	string gSenha(int id) => Senhas.First((s) => s.Id == id).Senha;
	string gHash(int id) => Senhas.First((s) => s.Id == id).Hash;

	static void AssertEhIgual<K>( IEnumerable<K> A, IEnumerable<K> B) 
		where K : IComparable<K>  
	{
		Assert.Equal(A.OrderBy(a=>a),B.OrderBy(b=>b));
	}
	public TestesIdentidade(ITestOutputHelper output)
	{
		BDU = new BDUtilSqlLite(LogUtils.CreateLoggerFactory(output));
		foreach (var perm in Permissoes.TodasPermissoes)
			perm.Cargos = new List<Cargo>();
		Senhas = new[]{
			(1, "admin"         ,"e00cf25ad42683b3df678c61f42c6bda"),
			(2, "2@oCP06io1#q"  , "97a290347762986f757e7fe694b43e45")
		};
		BDU.Cargos = new[]{
			new Cargo(IdCargoAdmin, "Administrador", Permissoes.TodasPermissoes.ToList()),
			new Cargo(IdCargoAnonimo, "Anonimo", new (){}),
		};
		BDU.Usuarios = new[]{
			new Usuario(IdAdmin	 , gHash(IdAdmin  ), "tavares", "Tavares" , BDU.gCargo(IdCargoAdmin)),
			new Usuario(IdAnonimo, gHash(IdAnonimo), "Atlas"  , "Atlas"   , BDU.gCargo(IdCargoAnonimo)),
		};
		BDU.SalvarDados();
		var BD = BDU.CriarContexto();
		var loggerFactory = LogUtils.CreateLoggerFactory(output);
		var repUsuarios =  new RepUsuarios(BD, loggerFactory.CreateLogger<RepUsuarios>());
		Identidade = new IdentidadeService(
			repUsuarios,
			new AutorizacaoService(repUsuarios, loggerFactory.CreateLogger<AutorizacaoService>()),
			new AutenticacaoService(repUsuarios, loggerFactory.CreateLogger<AutenticacaoService>()),
            loggerFactory.CreateLogger<IdentidadeService>()
		);
	}


	[Theory]
	[InlineData(IdAdmin)]
	[InlineData(IdAnonimo)]
	public async Task DefinirUsuario_UsuarioValidoAsync(int idUsuario)
	{
		await Identidade.DefinirUsuario(idUsuario);

		Assert.Equal(Identidade.IdUsuario, idUsuario);
	}

	[Theory]
	[InlineData(100)]
	[InlineData(-2)]
	public async Task DefinirUsuario_UsuarioInvalidoAsync(int idUsuario)
	{
		await Assert.ThrowsAsync<ArgumentException>(async ()=>{
			await Identidade.DefinirUsuario(idUsuario);
		});
	}

	[Fact]
	public async Task AutenticarUsuario_SenhaValidaAsync()
	{
		var idUsuario = IdAdmin;
		var UsuarioExperado =  BDU.gUsuario(idUsuario);
		
		await Identidade.DefinirUsuario(idUsuario);
		await Identidade.AutenticarUsuario(gSenha(idUsuario));
		var Usuario = Identidade.Usuario!;
		
		Assert.True(Identidade.EstaAutenticado);
		Assert.NotNull(Usuario);
		Assert.Equal(UsuarioExperado.Nome, Usuario.Nome);
		Assert.Equal(UsuarioExperado.Id, Usuario.Id);
		Assert.Equal(UsuarioExperado.Senha, Usuario.Senha);
		Assert.Equal(UsuarioExperado.Login, Usuario.Login);
		AssertEhIgual(UsuarioExperado.Cargo.Permissoes, Usuario.Cargo.Permissoes);
	}

	[Theory]
	[InlineData("admin1")]
	[InlineData("root")]
	public async Task AutenticarUsuario_SenhaInvalidaAsync(string senha)
	{
		var idUsuario = IdAdmin;

		await Identidade.DefinirUsuario(idUsuario);
		await Identidade.AutenticarUsuario(senha);

		Assert.False(Identidade.EstaAutenticado);
		Assert.NotEqual(Identidade.Usuario, BDU.gUsuario(IdAdmin));
	}
	[Fact]
	public async Task AutenticarUsuario_SenhaNulaAsync()
	{
		var idUsuario = IdAdmin;
		
		await Identidade.DefinirUsuario(idUsuario);

		await Assert.ThrowsAsync<ArgumentNullException>(async ()=>{
			await Identidade.AutenticarUsuario(null!);
		});
	}

	[Theory]
	[InlineData(IdAdmin  , true )]
	[InlineData(IdAnonimo, false)]
	public async Task EhAutorizadoAsync(int idUsuario, bool ExpectativaAutorizado )
	{
		var permissao = Permissoes.Cargo.Criar;
		await Identidade.DefinirUsuario(idUsuario);
		await Identidade.AutenticarUsuario(gSenha(idUsuario));

		var Autorizado = await Identidade.EhAutorizado(permissao);

		Assert.Equal(Autorizado, ExpectativaAutorizado);
	}

	[Fact]
	public async Task EhAutorizado_NaoAutenticadoAsync()
	{
		var permissao = Permissoes.Cargo.Criar;
		var idUsuario = IdAdmin;

		await Identidade.DefinirUsuario(idUsuario);
		
		bool Autorizado = await Identidade.EhAutorizado(permissao);

		Assert.False(Autorizado);
	}

	public void Dispose(){
		BDU.Dispose();
	}
}