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
	IdentidadeService CriarIdentidade()
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
	public void DefinirUsuario_UsuarioValido(int idUsuario)
	{
		var Identidade = CriarIdentidade();
		Identidade.DefinirUsuario(idUsuario);
		Assert.Equal(Identidade.IdUsuario, idUsuario);
	}

	[Theory]
	[InlineData(100)]
	[InlineData(-2)]
	public void DefinirUsuario_UsuarioInvalido(int idUsuario)
	{
		var Identidade = CriarIdentidade();
		Assert.Throws<ArgumentException>(()=>{
			Identidade.DefinirUsuario(idUsuario);
		});
	}

	[Fact]
	public void AutenticarUsuario_SenhaValida()
	{
		var idUsuario = IdAdmin;
		var Identidade = CriarIdentidade();
		Identidade.DefinirUsuario(idUsuario);

		Identidade.AutenticarUsuario(gSenha(idUsuario));
		Assert.True(Identidade.EstaAutenticado);
		Assert.Equal(Identidade.Usuario, gUsuario(IdAdmin));
	}

	[Theory]
	[InlineData("admin1")]
	[InlineData("root")]
	public void AutenticarUsuario_SenhaInvalida(string senha)
	{
		var idUsuario = IdAdmin;
		var Identidade = CriarIdentidade();
		Identidade.DefinirUsuario(idUsuario);
		Identidade.AutenticarUsuario(senha);
		Assert.False(Identidade.EstaAutenticado);
		Assert.NotEqual(Identidade.Usuario, gUsuario(IdAdmin));
	}
	[Fact]
	public void AutenticarUsuario_SenhaNula()
	{
		var idUsuario = IdAdmin;
		var Identidade = CriarIdentidade();
		Identidade.DefinirUsuario(idUsuario);
		Assert.Throws<ArgumentNullException>(()=>{
			Identidade.AutenticarUsuario(null!);
		});
	}

	[Theory]
	[InlineData(IdAdmin  , true )]
	[InlineData(IdAnonimo, false)]
	public void EhAutorizado(int idUsuario, bool ExpectativaAutorizado )
	{
		var permissao = Permissoes.Cargo.Criar;
		var Identidade = CriarIdentidade();
		Identidade.DefinirUsuario(idUsuario);
		Identidade.AutenticarUsuario(gSenha(idUsuario));

		var Autorizado = Identidade.EhAutorizado(permissao);

		Assert.Equal(Autorizado, ExpectativaAutorizado);
	}

	[Fact]
	public void EhAutorizado_NaoAutenticado()
	{
		var permissao = Permissoes.Cargo.Criar;
		var idUsuario = IdAdmin;
		var Identidade = CriarIdentidade();
		Identidade.DefinirUsuario(idUsuario);
		
		bool Autorizado = Identidade.EhAutorizado(permissao);

		Assert.False(Autorizado);
	}


}