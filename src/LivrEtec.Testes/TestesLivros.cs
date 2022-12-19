
using Grpc.Net.Client;
using LivrEtec.Servidor;
using LivrEtec.GIB;
using Microsoft.Extensions.Logging;
using Grpc.Core.Interceptors;

namespace LivrEtec.Testes;

[Collection("UsaBancoDeDados")]
public abstract class TestesLivro<T> : IClassFixture<ConfiguradorTestes> where T : IRepLivros
{
	protected abstract T RepLivros { get; }
	protected BDUtil BDU; 

	public static void AssertEhIgual<K>( IEnumerable<K> A, IEnumerable<K> B){
		Assert.Equal(new HashSet<K>(A),new HashSet<K>(B));
	}
	public TestesLivro(ConfiguradorTestes configurador) 
	{ 	
		BDU = new  BDUtil(configurador, (bdu)=>{
			bdu.Autores =  new Autor[]{
				new Autor(1, "J. R. R. Tolkien"),
				new Autor(2, "Friedrich Engels"),
				new Autor(3, "Karl Marx"),
				new Autor(4, "George Orwell")
			};

			bdu.Tags = new Tag[]{
				new Tag(1,"Aventura"),
				new Tag(2,"Fantasia"),
				new Tag(3,"Politica"),
				new Tag(4,"Literatura"),
				new Tag(5,"Sociologia"),
			};
			bdu.Livros =  new[]{
				new Livro {
					Id = 1,
					Nome = "Senhor dos Aneis",
					Arquivado = false,
					Autores = { bdu.gAutor(1) },
					Tags = { bdu.gTag(1), bdu.gTag(2),bdu.gTag(4)},
					Descricao = "Meu precioso"
				},
				new Livro {
					Id = 2,
					Nome = "O Capital",
					Arquivado = false,
					Autores = { bdu.gAutor(2), bdu.gAutor(3), },
					Tags = { bdu.gTag(3), bdu.gTag(2) },
					Descricao = "É tudo nosso"
				},
				new Livro { 
					Id = 3,
					Nome = "A Revolução dos Bixos",
					Arquivado = false,
					Autores = { bdu.gAutor(4)},
					Tags = { bdu.gTag(5) },
					Descricao = "É tudo nosso"
				}
			};
		});
	}
	[Fact]
	public async Task Registrar_LivroValidoAsync()
	{
		var idLivro = 5;
		var livroARegistrar  = new Livro{
			Id = idLivro,
			Nome = "Livro",
			Arquivado = true,
			Descricao = "Descrição",
			Tags =  {BDU.gTag(1), BDU.gTag(2) },
			Autores =  { BDU.gAutor(1) }
		};
		
		await RepLivros.RegistrarAsync(livroARegistrar);
		using var BD = BDU.CriarContexto();
		var livroRegistrado = BD.Livros.Find(idLivro)!;
		BD.Entry(livroRegistrado).Collection(l => l.Tags).Load();
		BD.Entry(livroRegistrado).Collection(l => l.Autores).Load();

		Assert.NotNull(livroRegistrado);
		Assert.Equal(livroARegistrar.Nome, livroRegistrado.Nome);
		Assert.Equal(livroARegistrar.Arquivado, livroRegistrado.Arquivado);
		Assert.Equal(livroARegistrar.Descricao, livroRegistrado.Descricao);
		AssertEhIgual(livroARegistrar.Tags, livroRegistrado.Tags);
		AssertEhIgual(livroARegistrar.Autores, livroRegistrado.Autores);
	}
	[Fact]
	public async Task Registrar_LivroExistenteAsync()
	{
		var livro  = BDU.gLivro(1);
		
		await Assert.ThrowsAsync<InvalidOperationException>(async ()=>{
			await RepLivros.RegistrarAsync(livro);
		});
	}
	[Fact]
	public async Task Registrar_idExistenteAsync(){
		var IdLivro = 1;
		var livro  = new Livro(){ 
			Id =  IdLivro,
			Nome = "douglas" 
		};

		await Assert.ThrowsAsync<InvalidOperationException>(async ()=>{
			await RepLivros.RegistrarAsync(livro);
		});

	}
	[Fact]
	public async Task Registrar_LivroNuloAsync()
	{
		Livro Livro =  null!;


		await Assert.ThrowsAsync<ArgumentNullException>(async ()=>{
			await RepLivros.RegistrarAsync(Livro);
		});
	}
	[Theory]
	[InlineData(-1, "nome")]
	[InlineData(10, "")]
	[InlineData(10, null)]
	public async Task Registrar_LivroInValidoAsync(int id, string nome)
	{
		var livro  = new Livro{
			Id = id,
			Nome = nome,
		};


		await Assert.ThrowsAsync<InvalidDataException>(async ()=>{
			await RepLivros.RegistrarAsync(livro);
		});
	}


	[Fact]
	public async Task Remover_LivroValidoAsync(){
		var Id =  1;
		
		await RepLivros.RemoverAsync(Id); 
		using var BD =  BDU.CriarContexto();
		var Contem =  BD.Livros.Any( l => l.Id == Id); 
		Assert.False(Contem);
	}
	[Fact]
	public async Task Remover_LivroInvalidoAsync(){
		var Id = 100;
	
		await Assert.ThrowsAsync<InvalidOperationException>(async ()=>{
			await RepLivros.RemoverAsync(Id);
		});
	}
	[Fact]
	public async Task Editar_TudoLivroValidoAsync()
	{
		var idLivro = 1;
		var livroEditado =  BDU.gLivro(idLivro)!;
		livroEditado.Nome = "Livro";
		livroEditado.Arquivado = true;
		livroEditado.Descricao = "Descrição";
		livroEditado.Tags =  new(){BDU.gTag(3) };
		livroEditado.Autores = new(){ BDU.gAutor(1) };
		using var BD =  BDU.CriarContexto();

		await RepLivros.EditarAsync(livroEditado);
		var livroRegistrado = BD.Livros.Find(idLivro)!;
		BD.Entry(livroRegistrado).Collection(l=> l.Tags).Load();
		BD.Entry(livroRegistrado).Collection(l=> l.Autores).Load();

		Assert.Equal(livroEditado.Nome, livroRegistrado.Nome);
		Assert.Equal( livroEditado.Arquivado, livroRegistrado.Arquivado);
		Assert.Equal( livroEditado.Descricao, livroRegistrado.Descricao);
		AssertEhIgual(livroEditado.Tags, livroRegistrado.Tags);
		AssertEhIgual(livroEditado.Autores, livroRegistrado.Autores);
	}


	[Fact]
	public async Task Editar_LivroNuloAsync()
	{
		Livro livro = null!;
		
		await Assert.ThrowsAsync<ArgumentNullException>(async ()=>{
			await RepLivros.EditarAsync(livro);
		});
	}

	[Fact]
	public async Task Editar_LivroTagNulaAsync()
	{
		var idLivro = 1;
		
		var livroEditado =  BDU.gLivro(idLivro)!;

		livroEditado.Tags = new List<Tag>(){ null! };
		await Assert.ThrowsAsync<ArgumentNullException>(async ()=>{
			await RepLivros.EditarAsync(livroEditado);
		});		
	}

	[Theory]
	[InlineData(""		, new int[]{}	, new int[]{1,2,3})]
	[InlineData(null	, null			, new int[]{1,2,3})]
	[InlineData(""		, new int[]{2,5}, new int[]{})]
	[InlineData(null	, new int[]{2,5}, new int[]{})]
	[InlineData("Senhor", new int[]{}	, new int[]{1})]
	[InlineData("Senhor", new int[]{2}	, new int[]{1})]
	public async Task Buscar_filtroValido(string textoBusca, int[] arrTag, int[] idExperados ){
		
		var resulutado=  RepLivros.BuscarAsync(textoBusca, textoBusca, arrTag?.Select(t=> BDU.gTag(t)));
		AssertEhIgual(idExperados, (await resulutado.ToArrayAsync()).Select((i)=> i.Id));
	}	
	
}
public class TestesLivrosLocal : TestesLivro<RepLivros>, IDisposable 
{
	AcervoService acervoService;
	PacaContext BD;
	protected override RepLivros RepLivros => (RepLivros)acervoService.Livros;
	public TestesLivrosLocal(ConfiguradorTestes configurador) : base(configurador)
	{
		BD = BDU.CriarContexto();
		acervoService = new AcervoService(BD, null);
	}
	
	public void Dispose(){
		BD.Dispose();
	}

}
public class TestesLivrosRPC: TestesLivro<RepLivroRPC>
{
	RepLivroRPC repLivrosRPC;
	protected override RepLivroRPC RepLivros => repLivrosRPC;
    public TestesLivrosRPC(ConfiguradorTestes configurador) : base(configurador)
	{
        var channel = GrpcChannel.ForAddress("http://localhost:5259");
        repLivrosRPC = new RepLivroRPC(configurador.loggerFactory.CreateLogger<RepLivroRPC>(),new GIB.RPC.Livros.LivrosClient(channel));
    }
}