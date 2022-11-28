
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes;
[Collection("UsaBancoDeDados")]
public class TestesAutenticacao  : TestesBD
{
	IAutenticacaoService AutenticacaoService;
	(int Id, string Senha, string Hash)[] Senhas; 
	Usuario gUsuario(int id) => Usuarios.First((u)=> u.Id == id); 
	string gSenha(int id) => Senhas.First((s)=> s.Id == id).Senha; 
	string gHash(int id) => Senhas.First((h)=> h.Id == id).Hash; 
	public TestesAutenticacao(ConfiguradorTestes configurador) : base(configurador)
	{ 	
		ResetarBanco();
		var Cargo = new Cargo(1, "cargo",new List<Permissao>());

		Senhas 	= new[]{
			(1, "Senha"			,"be6b9084a5dcdb09af8f433557a2119c"),
			(2, "Senha"			, "14621de3463eb7e1b3606d5514bbf800"),
			(3, "2@oCP06io1#q"	, "7b3608972fed79f056fe915e725f536e")
		};
		Usuarios =  new []{
			new Usuario(1, gHash(1), "tavares", "Tavares"	, Cargo),
			new Usuario(2, gHash(2), "Atlas"	, "Atlas"	, Cargo),
			new Usuario(3, gHash(3), "Atlas"	, "Atlas"	, Cargo),
		};
		BD.SaveChanges();
		AutenticacaoService = new AutenticacaoService(BD, loggerFactory.CreateLogger<AutenticacaoService>());
	}
	[Theory] 
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public void EhAutentico_Autentico(int idUsuario)
	{
		var senha =  gSenha(idUsuario);

		var autentico = AutenticacaoService.EhAutentico(idUsuario, senha);

		Assert.True(autentico);
	}

	[Theory] 
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public void EhAutentico_NaoAutentico(int idUsuario)
	{
		var senha =  "Qualquer senha aleatoria errada";

		var autentico = AutenticacaoService.EhAutentico(idUsuario, senha);

		Assert.False(autentico);
	}

	[Fact] 
	
	public void EhAutentico_UsuarioInvalido()
	{
		var senha =  "Qualquer senha aleatoria errada";
		var idUsuario = -10;

		Assert.Throws<ArgumentException>(()=>{
			AutenticacaoService.EhAutentico(idUsuario, senha);
		});
	}

	[Fact]
	public void EhAutentico_SenhaNula()
	{
		string senha = null!;
		var idUsuario = 1;

		Assert.Throws<ArgumentNullException>(()=>{
			AutenticacaoService.EhAutentico(idUsuario, senha);
		});
	} 
}