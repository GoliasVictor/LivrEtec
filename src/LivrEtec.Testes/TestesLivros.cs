

namespace LivrEtec.Testes;
[Collection("UsaBancoDeDados")]
public sealed class TestesLivro  : TestesBD
{
	AcervoService AcervoService;
	Autor gAutor(int id) => Autores.First((a)=> a.Id == id); 
	Tag gTag(int id) => Tags.First((a)=> a.Id == id); 
	Livro gLivro(int id) => Livros.First((l)=> l.Id == id); 
	public static bool EnumerableIgual<T>( IEnumerable<T> A, IEnumerable<T> B){
		return Enumerable.SequenceEqual(A.OrderBy((a)=>a),B.OrderBy(b=>b));
	}
	public TestesLivro(ConfiguradorTestes configurador) : base(configurador)
	{ 	
		ResetarBanco();
		Autores =  new Autor[]{
			new Autor(1, "J. R. R. Tolkien"),
			new Autor(2, "Friedrich Engels"),
			new Autor(3, "Karl Marx"),
			new Autor(4, "George Orwell")
		};

		Tags = new Tag[]{
			new Tag(1,"Aventura"),
            new Tag(2,"Fantasia"),
            new Tag(3,"Politica"),
            new Tag(4,"Literatura"),
            new Tag(5,"Sociologia"),
		};
		Livros =  new[]{
			new Livro {
				Id = 1,
				Nome = "Senhor dos Aneis",
				Arquivado = false,
				Autores = { gAutor(1) },
				Tags = { gTag(1), gTag(2), gTag(4)},
				Descricao = "Meu precioso"
			},
			new Livro {
				Id = 2,
				Nome = "O Capital",
				Arquivado = false,
				Autores = { gAutor(2), gAutor(3), },
				Tags = { gTag(3), gTag(2) },
				Descricao = "É tudo nosso"
			},
			new Livro { 
				Id = 3,
				Nome = "A Revolução dos Bixos",
				Arquivado = false,
				Autores = { gAutor(4)},
				Tags = { gTag(5) },
				Descricao = "É tudo nosso"
			}
		};
		BD.SaveChanges();
		AcervoService =  new AcervoService(BD, null);
	}
	[Fact]
	public void Registrar_LivroValido()
	{
		var idLivro = 5;
		var livroARegistrar  = new Livro{
			Id = idLivro,
			Nome = "Livro",
			Arquivado = true,
			Descricao = "Descrição",
			Tags =  {gTag(1), gTag(2) },
			Autores =  { gAutor(1) }
		};

		AcervoService.Livros.RegistrarAsync(livroARegistrar);

		var livroRegistrado = BD.Livros.Find(idLivro);

		Assert.True(
			livroARegistrar.Nome == livroRegistrado?.Nome
		 && livroARegistrar.Arquivado == livroRegistrado.Arquivado
		 && livroARegistrar.Descricao == livroRegistrado.Descricao
		 && EnumerableIgual(livroARegistrar.Tags, livroRegistrado.Tags)
		 && EnumerableIgual(livroARegistrar.Autores, livroRegistrado.Autores)
		);
	}
	[Fact]
	public async Task Registrar_LivroExistenteAsync()
	{
		var livro  = gLivro(1);
		
		await Assert.ThrowsAsync<InvalidOperationException>(async ()=>{
			await AcervoService.Livros.RegistrarAsync(livro);
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
			await AcervoService.Livros.RegistrarAsync(livro);
		});

	}
	[Fact]
	public async Task Registrar_LivroNuloAsync()
	{
		Livro Livro =  null!;


		await Assert.ThrowsAsync<ArgumentNullException>(async ()=>{
			await AcervoService.Livros.RegistrarAsync(Livro);
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
			await AcervoService.Livros.RegistrarAsync(livro);
		});
	}


	[Fact]
	public async Task Remover_LivroValidoAsync(){
		var livro =  gLivro(1);

		await AcervoService.Livros.RemoverAsync(livro);
		var Contem =  BD.Livros.Contains(livro); 
		
		Assert.False(Contem);
	}
	[Fact]
	public async Task Remover_LivroInvalidoAsync(){
		var livro =  new Livro(){ Id = 100 };

		await Assert.ThrowsAsync<InvalidOperationException>(async ()=>{
			await AcervoService.Livros.RemoverAsync(livro);
		});
	}
	[Fact]
	public async Task Editar_TudoLivroValidoAsync()
	{
		var idLivro = 1;
		var livroEditado =  gLivro(idLivro)!;
		livroEditado.Nome = "Livro";
		livroEditado.Arquivado = true;
		livroEditado.Descricao = "Descrição";
		livroEditado.Tags =  new(){gTag(3) };
		livroEditado.Autores = new(){ gAutor(1) };

		await AcervoService.Livros.EditarAsync(livroEditado);

		var livroRegistrado = BD.Livros.Find(idLivro)!;

		Assert.Equal(livroEditado.Nome, livroRegistrado.Nome);
		Assert.Equal( livroEditado.Arquivado, livroRegistrado.Arquivado);
		Assert.Equal( livroEditado.Descricao, livroRegistrado.Descricao);
		Assert.True( 
			EnumerableIgual(livroEditado.Tags, livroRegistrado.Tags)
		 && EnumerableIgual(livroEditado.Autores, livroRegistrado.Autores)
		);
	}


	[Fact]
	public async Task Editar_LivroNuloAsync()
	{
		Livro livro = null!;
		
		await Assert.ThrowsAsync<ArgumentNullException>(async ()=>{
			await AcervoService.Livros.EditarAsync(livro);
		});
	}

	[Fact]
	public async Task Editar_LivroTagNulaAsync()
	{
		var idLivro = 1;
		
		var livroEditado =  gLivro(idLivro)!;

		livroEditado.Tags = new List<Tag>(){ null! };
		await Assert.ThrowsAsync<ArgumentNullException>(async ()=>{
			await AcervoService.Livros.EditarAsync(livroEditado);
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
		
		var resulutado=  AcervoService.Livros.BuscarAsync(textoBusca, textoBusca, arrTag?.Select(t=> gTag(t)));
		var resultadoIgual = EnumerableIgual(idExperados, await resulutado.Select((i)=> i.Id).ToArrayAsync());
		Assert.True(resultadoIgual);
	}	
	
}