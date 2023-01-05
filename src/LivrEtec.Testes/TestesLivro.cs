using LivrEtec.Servidor;
using Grpc.Core.Interceptors;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace LivrEtec.Testes;

[Collection("UsaBancoDeDados")]
public abstract class TestesLivro<T> : IClassFixture<ConfiguradorTestes>  where T : IRepLivros  
{
	protected abstract T RepLivros { get;init;}
	protected readonly BDUtil BDU; 

	public static void AssertEhIgual<K>( IEnumerable<K> A, IEnumerable<K> B){
		Assert.Equal(new HashSet<K>(A),new HashSet<K>(B));
	}
    static void AssertLivroIgual(Livro livroEsperado, Livro livroAtual)
    {
        Assert.Equal(livroEsperado.Nome, livroAtual.Nome);
        Assert.Equal(livroEsperado.Arquivado, livroAtual.Arquivado);
        Assert.Equal(livroEsperado.Descricao, livroAtual.Descricao);
        AssertEhIgual(livroEsperado.Autores, livroAtual.Autores);
        AssertEhIgual(livroEsperado.Tags, livroAtual.Tags);
    }
	public TestesLivro(ConfiguradorTestes configurador, ITestOutputHelper output, BDUtil bdu)
	{
		BDU = bdu;
		BDU.Autores = new Autor[]{
				new Autor(1, "J. R. R. Tolkien"),
				new Autor(2, "Friedrich Engels"),
				new Autor(3, "Karl Marx"),
				new Autor(4, "George Orwell")
			};

		BDU.Tags = new Tag[]{
			new Tag(1,"Aventura"),
			new Tag(2,"Fantasia"),
			new Tag(3,"Politica"),
			new Tag(4,"Literatura"),
			new Tag(5,"Sociologia"),
		}; 
		
		BDU.Livros = new[]{
			new Livro {
				Id = 1,
				Nome = "Senhor dos Aneis",
				Arquivado = false,
				Autores = { BDU.gAutor(1) },
				Tags = { BDU.gTag(1), BDU.gTag(2),BDU.gTag(4)},
				Descricao = "Meu precioso", 
				Quantidade = 10
			},
			new Livro {
				Id = 2,
				Nome = "O Capital",
				Arquivado = false,
				Autores = { BDU.gAutor(2), BDU.gAutor(3), },
				Tags = { BDU.gTag(3), BDU.gTag(2) },
				Descricao = "É tudo nosso",
				Quantidade = 5
			},
			new Livro {
				Id = 3,
				Nome = "A Revolução dos Bixos",
				Arquivado = false,
				Autores = { BDU.gAutor(4)},
				Tags = { BDU.gTag(5) },
				Descricao = "É tudo nosso",
				Quantidade = 1
			}
		};
		BDU.SalvarDados();
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
			Autores =  { BDU.gAutor(1) },
			Quantidade = int.MaxValue
		};
		
		await RepLivros.RegistrarAsync(livroARegistrar);
		using var BD = BDU.CriarContexto();
		var livroRegistrado = BD.Livros.Find(idLivro)!;
		BD.Entry(livroRegistrado).Collection(l => l.Tags).Load();
		BD.Entry(livroRegistrado).Collection(l => l.Autores).Load();
		
		Assert.NotNull(livroRegistrado);
		AssertLivroIgual(livroARegistrar,livroRegistrado);
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
			Nome = "douglas",
			Quantidade = 1
		};

		await Assert.ThrowsAsync<InvalidOperationException>(async ()=>{
			await RepLivros.RegistrarAsync(livro);
		});

	}
	[Fact]
	public async Task Registrar_LivroNuloAsync()
	{
		Livro Livro =  null!;


		await Assert.ThrowsAsync<ValidationException>(async ()=>{
			await RepLivros.RegistrarAsync(Livro);
		});
	}
	[Theory]
	[InlineData(-1, 1, "nome")]
	[InlineData(10,-1, "nome")]
	[InlineData(10, 0, "nome")]
	[InlineData(10,1, "")]
	[InlineData(10,1, null)]
	public async Task Registrar_LivroInvalidoAsync(int id, int Quantidade, string nome)
	{
		var livro  = new Livro{
			Id = id,
			Nome = nome,
			Quantidade = Quantidade
		};


		await Assert.ThrowsAsync<ValidationException>(async ()=>{
			await RepLivros.RegistrarAsync(livro);
		});
	}


	[Fact]
	public async Task Remover_LivroValidoAsync(){
		var Id =  1;
		
		await RepLivros.RemoverAsync(Id); 
		using var BD = BDU.CriarContexto();
		var Contem = BD.Livros.Any( l => l.Id == Id); 
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
		var livroEditado = BDU.gLivro(idLivro)!;
		livroEditado.Nome = "Livro";
		livroEditado.Arquivado = true;
		livroEditado.Descricao = "Descrição";
		livroEditado.Tags = new() { BDU.gTag(3) };
		livroEditado.Autores = new() { BDU.gAutor(2) };
		var livroEsperado = livroEditado.Clone();
		using var BD = BDU.CriarContexto();

		await RepLivros.EditarAsync(livroEditado);
		var livroRegistrado = BD.Livros.Find(idLivro)!;
		BD.Entry(livroRegistrado).Collection(l => l.Tags).Load();
		BD.Entry(livroRegistrado).Collection(l => l.Autores).Load();

		AssertLivroIgual(livroEsperado, livroRegistrado);
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
		await Assert.ThrowsAsync<InvalidDataException>(async ()=>{
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
		
		var resulutado=  await RepLivros.BuscarAsync(textoBusca, textoBusca, arrTag);
		AssertEhIgual(idExperados,  resulutado.Select((i)=> i.Id));
	}	
	
}
