using Xunit.Abstractions;
namespace LivrEtec.Testes;


[Collection("UsaBancoDeDados")]
public abstract class TestesEmprestimoService<T> where T : IEmprestimoService
{
	protected readonly BDUtil BDU;
	protected abstract T emprestimoService { get; init; }
	protected readonly IRelogio relogio;
	protected const int ID_PESSOA = 1;
	protected const int ID_ALUNO = 2;
	protected const int ID_USUARIO_CRIADOR = 1;
	protected const int ID_USUARIO_TESTE = 2;
	protected const int ID_LIVRO_DISPONIVEL = 1;
	protected const int ID_EMPRESTIMO_ABERTO = 2;
	protected const int ID_EMPRESTIMO_FECHADO = 1;
	protected readonly Usuario usuarioTeste;
	public TestesEmprestimoService(ITestOutputHelper output, IRelogio relogio, BDUtil bdu)
	{
		BDU = bdu;
		this.relogio = relogio;
		var cargoTeste = new Cargo()
		{
			Id = 10,
			Nome = "Cargo Teste",
			Permissoes = Permissoes.TodasPermissoes.ToList()
		};
		usuarioTeste = new Usuario()
		{
			Id = ID_USUARIO_TESTE,
			Nome = "Usuario Teste Emprestimo Service",
			Login = "teste",
			Senha = "senha",
			Cargo = cargoTeste
		};

		BDU.Autores = new Autor[]{
				new Autor(1, "J. R. R. Tolkien"),
				new Autor(2, "Friedrich Engels")
			};

		BDU.Tags = new Tag[]{
				new Tag(1,"Aventura"),
				new Tag(2,"Fantasia"),
				new Tag(3,"Politica")
			};
		BDU.Cargos = new[] { usuarioTeste.Cargo };
		foreach (Permissao perm in Permissoes.TodasPermissoes)
		{
			perm.Cargos = new List<Cargo>();
		}

		BDU.Usuarios = new[]{
				new Usuario(){
					Id= ID_USUARIO_CRIADOR,
					Nome="Usuario criador",
					Senha= IAutenticacaoService.GerarHahSenha(ID_USUARIO_CRIADOR, "Senha"),
					Login="usuario_criador",
					Cargo =  cargoTeste,
				},
				usuarioTeste
			};
		BDU.Livros = new[]{
				new Livro {
					Id = ID_LIVRO_DISPONIVEL,
					Nome = "O Capital",
					Arquivado = false,
					Autores = { BDU.gAutor(2), BDU.gAutor(1), },
					Tags = { BDU.gTag(1), BDU.gTag(3) },
					Descricao = "É tudo nosso",
					Quantidade = 2
				},
				new Livro {
					Id = 3,
					Nome = "A Revolução dos Bixos",
					Arquivado = false,
					Autores = { BDU.gAutor(2)},
					Tags = { BDU.gTag(2) },
					Descricao = "É tudo nosso",
					Quantidade = 1
				}
			};
		BDU.Pessoas = new[]
		{
				new Pessoa(){
					Id = ID_PESSOA,
					Nome = "carlos",
					Telefone = "13995789636"
				},
				new Aluno(){
					Id = ID_ALUNO,
					Nome = "joão",
					RM="102433",
					Telefone="13999822353"
				}
			};
		BDU.Emprestimos = new[]{
				new Emprestimo(){
					Id= ID_EMPRESTIMO_FECHADO,
					Livro = BDU.gLivro(1),
					Pessoa = BDU.gPessoa(1),
					UsuarioCriador = BDU.gUsuario(ID_USUARIO_CRIADOR),
					UsuarioFechador = BDU.gUsuario(ID_USUARIO_CRIADOR),
					Fechado = true,
					AtrasoJustificado = false,
					Comentario="",
					DataEmprestimo= relogio.Agora.AddMonths(-11),
					DataFechamento = relogio.Agora.AddMonths(-11).AddDays(15),
					FimDataEmprestimo= relogio.Agora.AddMonths(-10),
					ExplicacaoAtraso= null,
				},
				new Emprestimo(){
					Id= ID_EMPRESTIMO_ABERTO,
					Livro = BDU.gLivro(3),
					Pessoa = BDU.gPessoa(ID_ALUNO),
					UsuarioCriador = BDU.gUsuario(ID_USUARIO_CRIADOR),
					DataEmprestimo= relogio.Agora.AddDays(-15),
					FimDataEmprestimo= relogio.Agora.AddDays(15),
					Fechado = false,
					Comentario="",
					AtrasoJustificado = null,
					DataFechamento=null,
					ExplicacaoAtraso= null,
				}
			};
		BDU.SalvarDados();
		_ = BDU.CriarContexto();
	}
	private void AssertEmprestimoIgual(Emprestimo esperado, Emprestimo atual)
	{
		Assert.Equal(esperado.AtrasoJustificado, atual.AtrasoJustificado);
		Assert.Equal(esperado.Comentario, atual.Comentario);
		Assert.Equal(esperado.DataEmprestimo, atual.DataEmprestimo, new TimeSpan(1, 0, 0, 0));
		if (esperado.DataFechamento is not null && atual.DataFechamento is not null)
		{
			Assert.Equal(esperado.DataFechamento.Value, atual.DataFechamento.Value, new TimeSpan(1, 0, 0, 0));
		}
		else
		{
			Assert.Equal(esperado.DataFechamento, atual.DataFechamento);
		}

		Assert.Equal(esperado.Devolvido, atual.Devolvido);
		Assert.Equal(esperado.ExplicacaoAtraso, atual.ExplicacaoAtraso);
		Assert.Equal(esperado.Fechado, atual.Fechado);
		Assert.Equal(esperado.FimDataEmprestimo, atual.FimDataEmprestimo, new TimeSpan(1, 0, 0, 0));
		Assert.Equal(esperado.Id, atual.Id);
		Assert.Equal(esperado.Livro.Id, atual.Livro.Id);
		Assert.Equal(esperado.Pessoa.Id, atual.Pessoa.Id);
		Assert.Equal(esperado.UsuarioCriador.Id, atual.UsuarioCriador?.Id);
		Assert.Equal(esperado.UsuarioFechador?.Id, atual.UsuarioFechador?.Id);
	}

	[Fact]
	public async Task AbrirAsync_Valido()
	{
		const int idPessoa = ID_ALUNO;
		var emprestimoEsperado = new Emprestimo()
		{
			Pessoa = BDU.gPessoa(idPessoa),
			Livro = BDU.gLivro(ID_LIVRO_DISPONIVEL),
			UsuarioCriador = BDU.gUsuario(ID_USUARIO_TESTE),
			DataEmprestimo = relogio.Agora,
			Fechado = false,
			Comentario = null,
			DataFechamento = null,
			AtrasoJustificado = null,
			ExplicacaoAtraso = null,
			FimDataEmprestimo = relogio.Agora.AddDays(30),
		};

		var idEmprestimo = await emprestimoService.Abrir(idPessoa, ID_LIVRO_DISPONIVEL);

		emprestimoEsperado.Id = idEmprestimo;
		Emprestimo? emprestimoAtual = await BDU.gEmprestimoBanco(idEmprestimo);
		Assert.NotNull(emprestimoAtual);
		AssertEmprestimoIgual(emprestimoEsperado, emprestimoAtual!);

	}

	[Fact]
	public async Task ProrrogarAsnc_Valido()
	{
		var dataEsperada = new DateTime(2022, 3, 1);
		var idEmprestimo = ID_EMPRESTIMO_ABERTO;
		Emprestimo emprestimoEsperado = BDU.gEmprestimo(idEmprestimo);
		emprestimoEsperado.FimDataEmprestimo = dataEsperada;

		await emprestimoService.Prorrogar(idEmprestimo, dataEsperada);

		Emprestimo? emprestimoAtual = await BDU.gEmprestimoBanco(idEmprestimo);
		Assert.NotNull(emprestimoAtual);
		AssertEmprestimoIgual(emprestimoEsperado, emprestimoAtual!);
	}



	[Fact]
	public async void DevolverAsync_SemAtraso()
	{
		var idEmprestimo = ID_EMPRESTIMO_ABERTO;
		Emprestimo emprestimoEsperado = BDU.gEmprestimo(idEmprestimo);
		emprestimoEsperado.Devolvido = true;
		emprestimoEsperado.Fechado = true;
		emprestimoEsperado.UsuarioFechador = BDU.gUsuario(ID_USUARIO_TESTE);
		emprestimoEsperado.DataFechamento = relogio.Agora;

		await emprestimoService.Devolver(idEmprestimo);

		Emprestimo? emprestimoAtual = await BDU.gEmprestimoBanco(idEmprestimo);
		Assert.NotNull(emprestimoAtual);
		AssertEmprestimoIgual(emprestimoEsperado, emprestimoAtual!);

	}

	[Fact]
	public async void DevolverAsync_ComAtrasoJustificado()
	{
		const int idEmprestimo = ID_EMPRESTIMO_ABERTO;
		const string ExplicacaoAtraso = "Por motivos de teste.";
		Emprestimo emprestimoEsperado = BDU.gEmprestimo(idEmprestimo);
		emprestimoEsperado.Devolvido = true;
		emprestimoEsperado.Fechado = true;
		emprestimoEsperado.AtrasoJustificado = true;
		emprestimoEsperado.ExplicacaoAtraso = ExplicacaoAtraso;
		emprestimoEsperado.UsuarioFechador = BDU.gUsuario(ID_USUARIO_TESTE);
		emprestimoEsperado.DataFechamento = relogio.Agora;

		await emprestimoService.Devolver(idEmprestimo, AtrasoJustificado: true, ExplicacaoAtraso);

		Emprestimo? emprestimoAtual = await BDU.gEmprestimoBanco(idEmprestimo);
		Assert.NotNull(emprestimoAtual);
		AssertEmprestimoIgual(emprestimoEsperado, emprestimoAtual!);

	}

	[Fact]
	public async Task RegistrarPerdaAsync_ValidaAsync()
	{
		var idEmprestimo = ID_EMPRESTIMO_ABERTO;
		
		Emprestimo emprestimoEsperado = BDU.gEmprestimo(idEmprestimo);
		emprestimoEsperado.Devolvido = false;
		emprestimoEsperado.Fechado = true;
		emprestimoEsperado.DataFechamento = relogio.Agora;
		emprestimoEsperado.UsuarioFechador = BDU.gUsuario(ID_USUARIO_TESTE);
		var qtLivroEsperado = emprestimoEsperado.Livro.Quantidade - 1;
		
		await emprestimoService.RegistrarPerda(idEmprestimo);

		Emprestimo? emprestimoAtual = await BDU.gEmprestimoBanco(idEmprestimo);
		Assert.NotNull(emprestimoAtual);
		Assert.Equal(qtLivroEsperado, emprestimoAtual?.Livro.Quantidade);
		AssertEmprestimoIgual(emprestimoEsperado, emprestimoAtual!);
		
	}
	[Fact]
	public async Task ExcluirAsync_ValidaAsync()
	{
		var idEmprestimo = ID_EMPRESTIMO_ABERTO;

		await emprestimoService.Excluir(idEmprestimo);

		Emprestimo? emprestimoAtual = await BDU.gEmprestimoBanco(idEmprestimo);
		Assert.Null(emprestimoAtual);
	}
}
