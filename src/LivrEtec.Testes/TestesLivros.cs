
using Microsoft.Extensions.Logging;

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

		AcervoService.Livros.Registrar(livroARegistrar);

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
	public void Registrar_LivroExistente()
	{
		var livro  = gLivro(1);
		
		Assert.Throws<InvalidOperationException>(()=>{
			AcervoService.Livros.Registrar(livro);
		});
	}
	[Fact]
	public void Registrar_idExistente(){
		var IdLivro = 1;
		var livro  = new Livro(){ 
			Id =  IdLivro,
			Nome = "douglas" 
		};

		Assert.Throws<InvalidOperationException>(()=>{
			AcervoService.Livros.Registrar(livro);
		});

	}
	[Fact]
	public void Registrar_LivroNulo()
	{
		Livro Livro =  null!;


		Assert.Throws<ArgumentNullException>(()=>{
			AcervoService.Livros.Registrar(Livro);
		});
	}
	[Theory]
	[InlineData(-1, "nome")]
	[InlineData(10, "")]
	[InlineData(10, null)]
	public void Registrar_LivroInValido(int id, string nome)
	{
		var livro  = new Livro{
			Id = id,
			Nome = nome,
		};


		Assert.Throws<InvalidDataException>(()=>{
			AcervoService.Livros.Registrar(livro);
		});
	}


	[Fact]
	public void Remover_LivroValido(){
		var livro =  gLivro(1);

		AcervoService.Livros.Remover(livro);
		var Contem =  BD.Livros.Contains(livro); 
		
		Assert.False(Contem);
	}
	[Fact]
	public void Remover_LivroInvalido(){
		var livro =  new Livro(){ Id = 100 };

		Assert.Throws<InvalidOperationException>(()=>{
			AcervoService.Livros.Remover(livro);
		});
	}
	[Fact]
	public void Editar_TudoLivroValido()
	{
		var idLivro = 1;
		var livroEditado =  gLivro(idLivro)!;
		livroEditado.Nome = "Livro";
		livroEditado.Arquivado = true;
		livroEditado.Descricao = "Descrição";
		livroEditado.Tags =  new(){gTag(3) };
		livroEditado.Autores = new(){ gAutor(1) };

		AcervoService.Livros.Editar(livroEditado);

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
	public void Editar_LivroNulo()
	{
		Livro livro = null!;
		
		Assert.Throws<ArgumentNullException>(()=>{
			AcervoService.Livros.Editar(livro);
		});
	}

	[Fact]
	public void Editar_LivroTagNula()
	{
		var idLivro = 1;
		
		var livroEditado =  gLivro(idLivro)!;

		livroEditado.Tags = new List<Tag>(){ null! };
		Assert.Throws<ArgumentNullException>(()=>{
			AcervoService.Livros.Editar(livroEditado);
		});		
	}

	[Theory]
	[InlineData(""		, new int[]{}	, new int[]{1,2,3})]
	[InlineData(null	, null			, new int[]{1,2,3})]
	[InlineData(""		, new int[]{2,5}, new int[]{})]
	[InlineData(null	, new int[]{2,5}, new int[]{})]
	[InlineData("Senhor", new int[]{}	, new int[]{1})]
	[InlineData("Senhor", new int[]{2}	, new int[]{1})]
	public void Buscar_filtroValido(string textoBusca, int[] arrTag, int[] idExperados ){
		
		var resulutado= AcervoService.Livros.Buscar(textoBusca, textoBusca, arrTag?.Select(t=> gTag(t)));
		var resultadoIgual = EnumerableIgual(idExperados, resulutado.Select((i)=> i.Id));
		Assert.True(resultadoIgual);
	}	
	
}