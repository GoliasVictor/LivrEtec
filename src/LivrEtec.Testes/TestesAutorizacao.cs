using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes;
[Collection("UsaBancoDeDados")]
public class TestesAutorizacao  :TestesBD
{
	const int IdAdministrador = 1;
	const int IdAnonimo = 2;
	IAutorizacaoService AutorizacaoService;
	Usuario gUsuario(int id) => Usuarios.First((u)=> u.Id == id); 
	Cargo gCargo(int id) => Cargos.First((c)=> c.Id == id); 
	public TestesAutorizacao(ConfiguradorTestes configurador) : base(configurador)
	{ 	
        ResetarBanco();
		foreach (var perm in Permissoes.TodasPermissoes)
			perm.Cargos = new List<Cargo>();
		BDPermissoes =  Permissoes.TodasPermissoes;
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
			new Usuario(1, "", "tavares", "Tavares"	, gCargo(IdAdministrador)),
			new Usuario(2, "", "Ze"		, "Zé"		, gCargo(IdAnonimo)),
			new Usuario(3, "", "Paca"	, "Paca"	, gCargo(3)),
			new Usuario(4, "", "Atlas"	, "Atlas"	, gCargo(4)),
		};
	
		BD.SaveChanges();
		AutorizacaoService =  new AutorizacaoService(BD, loggerFactory.CreateLogger<AutorizacaoService>());
	}
	[Theory]
	[InlineData(IdAdministrador)]
	[InlineData(3)]
	[InlineData(4)]
	public async void EhAutorizado_Autorizado(int idUsuario)
	{
		var usuario = gUsuario(idUsuario);
		var permissao =  Permissoes.Livro.Visualizar;

		var autorizado = await AutorizacaoService.EhAutorizadoAsync(usuario, permissao);

		Assert.True(autorizado);
	}
	[Theory]
	[InlineData(IdAnonimo)]
	[InlineData(3)]
	[InlineData(4)]
	public async void EhAutorizado_NaoAutorizado(int idUsuario)
	{
		var usuario = gUsuario(idUsuario);
		var permissao =  Permissoes.Cargo.Criar;

		var autorizado = await AutorizacaoService.EhAutorizadoAsync(usuario, permissao);
		
		Assert.False(autorizado);
	}

    [Fact]
    public async Task ErroSeNaoAutorizado_NadaAsync()
    {
        var usuario = gUsuario(IdAdministrador);
        var permissao = Permissoes.Cargo.Criar;
        await AutorizacaoService.ErroSeNaoAutorizadoAsync(usuario, permissao);
    }
    [Fact]
    public async Task ErroSeNaoAutorizado_NaoAutorizadoExceptionAsync()
    {
        var usuario = gUsuario(IdAnonimo);
        var permissao = Permissoes.Cargo.Criar;
		await Assert.ThrowsAsync<NaoAutorizadoException>(async () => {
			await AutorizacaoService.ErroSeNaoAutorizadoAsync(usuario, permissao);
		});
    }

	[Fact]
    public async Task ErroSeNaoAutorizado_PermissaoNulaAsync()
    {
        var usuario = gUsuario(IdAnonimo);
        Permissao permissao = null!;
		await Assert.ThrowsAsync<ArgumentNullException>(async () => {
			await AutorizacaoService.ErroSeNaoAutorizadoAsync(usuario, permissao);
		});
    }
	[Fact]
    public async Task ErroSeNaoAutorizado_PermissaoInvalidaAsync()
    {
        var usuario = gUsuario(IdAnonimo);
		const int IdInvalido = 100;
		Permissao permissao = new Permissao(){ Id = IdInvalido };
		await Assert.ThrowsAsync<ArgumentException>(async () => {
			await AutorizacaoService.ErroSeNaoAutorizadoAsync(usuario, permissao);
		});
    }

}