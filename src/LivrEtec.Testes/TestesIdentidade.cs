using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes;
[Collection("UsaBancoDeDados")]
public class TestesIdentidade : TestesBD
{
	const int IdCargoAdmin = 1;
	const int IdCargoAnonimo = 2;
	const int IdAdmin = 1;
	const int IdAnonimo = 2;
	IAutorizacaoService AutorizacaoService;
	IAutenticacaoService AutenticacaoService;
	(int Id, string Senha, string Hash)[] Senhas;
	Usuario gUsuario(int id) => Usuarios.First((u) => u.Id == id);
	string gSenha(int id) => Senhas.First((s) => s.Id == id).Senha;
	string gHash(int id) => Senhas.First((s) => s.Id == id).Hash;
	Cargo gCargo(int id) => Cargos.First((c)=> c.Id == id); 
	IIdentidadeService CriarIdentidade()
	{
		return new IdentidadeService(
			BD,
			loggerFactory.CreateLogger<IdentidadeService>(),
			AutorizacaoService,
			AutenticacaoService
		);
	}
	public TestesIdentidade(ConfiguradorTestes configurador) : base(configurador)
	{
		ResetarBanco();
		foreach (var perm in Permissoes.TodasPermissoes)
			perm.Cargos = new List<Cargo>();
		Senhas = new[]{
			(1, "admin"         ,"e00cf25ad42683b3df678c61f42c6bda"),
			(2, "2@oCP06io1#q"  , "97a290347762986f757e7fe694b43e45")
		};
		Cargos = new[]{
			new Cargo(IdCargoAdmin, "Administrador", Permissoes.TodasPermissoes.ToList()),
			new Cargo(IdCargoAnonimo, "Anonimo", new (){}),
		};
		Usuarios = new[]{
			new Usuario(IdAdmin	 , gHash(IdAdmin  ), "tavares", "Tavares" , gCargo(IdCargoAdmin)),
			new Usuario(IdAnonimo, gHash(IdAnonimo), "Atlas"  , "Atlas"   , gCargo(IdCargoAnonimo)),
		};

		BD.SaveChanges();
		AutenticacaoService = new AutenticacaoService(BD, loggerFactory.CreateLogger<AutenticacaoService>());
		AutorizacaoService = new AutorizacaoService(BD, loggerFactory.CreateLogger<AutorizacaoService>());

	}


	[Theory]
	[InlineData(IdAdmin)]
	[InlineData(IdAnonimo)]
	public async Task DefinirUsuario_UsuarioValidoAsync(int idUsuario)
	{
		var Identidade = CriarIdentidade();
		await Identidade.DefinirUsuarioAsync(idUsuario);
		Assert.Equal(Identidade.IdUsuario, idUsuario);
	}

	[Theory]
	[InlineData(100)]
	[InlineData(-2)]
	public async Task DefinirUsuario_UsuarioInvalidoAsync(int idUsuario)
	{
		var Identidade = CriarIdentidade();
		await Assert.ThrowsAsync<ArgumentException>(async ()=>{
			await Identidade.DefinirUsuarioAsync(idUsuario);
		});
	}

	[Fact]
	public async Task AutenticarUsuario_SenhaValidaAsync()
	{
		var idUsuario = IdAdmin;
		var Identidade = CriarIdentidade();
		await Identidade.DefinirUsuarioAsync(idUsuario);
		await Identidade.AutenticarUsuarioAsync(gSenha(idUsuario));
	
		Assert.True(Identidade.EstaAutenticado);
		Assert.Equal(Identidade.Usuario, gUsuario(IdAdmin));
	}

	[Theory]
	[InlineData("admin1")]
	[InlineData("root")]
	public async Task AutenticarUsuario_SenhaInvalidaAsync(string senha)
	{
		var idUsuario = IdAdmin;
		var Identidade = CriarIdentidade();
		await Identidade.DefinirUsuarioAsync(idUsuario);
		await Identidade.AutenticarUsuarioAsync(senha);
		Assert.False(Identidade.EstaAutenticado);
		Assert.NotEqual(Identidade.Usuario, gUsuario(IdAdmin));
	}
	[Fact]
	public async Task AutenticarUsuario_SenhaNulaAsync()
	{
		var idUsuario = IdAdmin;
		var Identidade = CriarIdentidade();
		await Identidade.DefinirUsuarioAsync(idUsuario);
		await Assert.ThrowsAsync<ArgumentNullException>(async ()=>{
			await Identidade.AutenticarUsuarioAsync(null!);
		});
	}

	[Theory]
	[InlineData(IdAdmin  , true )]
	[InlineData(IdAnonimo, false)]
	public async Task EhAutorizadoAsync(int idUsuario, bool ExpectativaAutorizado )
	{
		var permissao = Permissoes.Cargo.Criar;
		var Identidade = CriarIdentidade();
		await Identidade.DefinirUsuarioAsync(idUsuario);
		await Identidade.AutenticarUsuarioAsync(gSenha(idUsuario));

		var Autorizado = await Identidade.EhAutorizadoAsync(permissao);

		Assert.Equal(Autorizado, ExpectativaAutorizado);
	}

	[Fact]
	public async Task EhAutorizado_NaoAutenticadoAsync()
	{
		var permissao = Permissoes.Cargo.Criar;
		var idUsuario = IdAdmin;
		var Identidade = CriarIdentidade();
		await Identidade.DefinirUsuarioAsync(idUsuario);
		
		bool Autorizado = await Identidade.EhAutorizadoAsync(permissao);

		Assert.False(Autorizado);
	}


}