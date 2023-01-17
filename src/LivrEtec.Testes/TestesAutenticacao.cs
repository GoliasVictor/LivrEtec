using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LivrEtec.Testes.Local;
[Collection("UsaBancoDeDados")]
[Trait("Category", "Local")]
public class TestesAutenticacao : IDisposable
{
	readonly BDUtil BDU;
	readonly PacaContext BD;
	readonly IAutenticacaoService autenticacaoService;
	(int Id, string Senha, string Hash)[] Senhas; 
	string gSenha(int id) => Senhas.First((s)=> s.Id == id).Senha; 
	string gHash(int id) => Senhas.First((h)=> h.Id == id).Hash; 
	public TestesAutenticacao(ITestOutputHelper output)
	{
		var loggerFactory = LogUtils.CreateLoggerFactory(output);
		BDU = new BDUtilSqlLite(loggerFactory);
		var Cargo = new Cargo(1, "cargo",new List<Permissao>());
		Senhas 	= new[]{
			(1, "Senha"			,"be6b9084a5dcdb09af8f433557a2119c"),
			(2, "Senha"			, "14621de3463eb7e1b3606d5514bbf800"),
			(3, "2@oCP06io1#q"	, "7b3608972fed79f056fe915e725f536e")
		};
		BDU.Usuarios = new []{
			new Usuario(1, gHash(1), "tavares", "Tavares"	, Cargo),
			new Usuario(2, gHash(2), "Atlas"	, "Atlas"	, Cargo),
			new Usuario(3, gHash(3), "Atlas"	, "Atlas"	, Cargo),
		};
		BDU.SalvarDados();
		BD = BDU.CriarContexto();
		
		var repUsuarios =  new RepUsuarios(BD, LogUtils.CreateLogger<RepUsuarios>(output));
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
		var senha =  gSenha(idUsuario);

		var autentico = await autenticacaoService.EhAutenticoAsync(idUsuario, AutenticacaoService.GerarHahSenha(idUsuario,senha));

		Assert.True(autentico);
	}

	[Theory] 
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public async Task EhAutentico_NaoAutenticoAsync(int idUsuario)
	{
		var senha =  "Qualquer senha aleatoria errada";

		var autentico = await autenticacaoService.EhAutenticoAsync(idUsuario, senha);

		Assert.False(autentico);
	}

	[Fact] 
	
	public async Task EhAutentico_UsuarioInvalidoAsync()
	{
		var senha =  "Qualquer senha aleatoria errada";
		var idUsuario = -10;

		await Assert.ThrowsAsync<ArgumentException>(async ()=>{
			await autenticacaoService.EhAutenticoAsync(idUsuario, senha);
		});
	}

	[Fact]
	public async Task EhAutentico_SenhaNulaAsync()
	{
		string senha = null!;
		var idUsuario = 1;

		await Assert.ThrowsAsync<ArgumentNullException>(async ()=>{
			await autenticacaoService.EhAutenticoAsync(idUsuario, senha);
		});
	}

	public void Dispose()
	{
		BD.Dispose();
	}
}