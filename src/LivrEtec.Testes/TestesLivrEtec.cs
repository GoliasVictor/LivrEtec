
using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes;

public class TestesLivro  : IClassFixture<ConfiguradorTestes>
{
	PacaContext BD;
	AcervoService AcervoService;
	Tag[] Tags;
	Autor[] Autores;
	Livro[] Livros;
	Autor gAutor(int id) => Autores.First((a)=> a.Id == id); 
	Tag gTag(int id) => Tags.First((a)=> a.Id == id); 
	Tag gLivro(int id) => Tags.First((a)=> a.Id == id); 
	public static bool EnumerableIgual<T>( IEnumerable<T> A, IEnumerable<T> B){
		return Enumerable.SequenceEqual(A.OrderBy((a)=>a),B.OrderBy(b=>b));
	}
	public TestesLivro(ConfiguradorTestes configurador)
	{ 	

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
				Nome = "Senhor dos Aneis",
				Arquivado = false,
				Autores = { gAutor(1) },
				Tags = { gTag(1), gTag(2), gTag(4)},
				Descricao = "Meu precioso"
			},
			new Livro {
				Nome = "O Capital",
				Arquivado = false,
				Autores = { gAutor(2), gAutor(3), },
				Tags = { gTag(3) },
				Descricao = "É tudo nosso"
			},
			new Livro {
				Nome = "A Revolução dos Bixos",
				Arquivado = false,
				Autores = { gAutor(4)},
				Tags = { gTag(5) },
				Descricao = "É tudo nosso"
			}
		};

		//BD = new PacaContext(LoggerFactory.Create((lb)=> { 
		//	lb.AddConsole();
		//	lb.AddFilter((_,_, logLevel)=> logLevel >= LogLevel.Information);
		//}));
		BD = new PacaContext(configurador.Config);

        BD.Database.EnsureDeleted();
        BD.Database.EnsureCreated();
		BD.Autores.AddRange(Autores);
		BD.Tags.AddRange(Tags);
		BD.Livros.AddRange(Livros);
		BD.SaveChanges();
		

		BD.SaveChanges();
		AcervoService =  new AcervoService(BD,null);
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

	[Theory]
	[InlineData(-1, "nome")]
	[InlineData(1, "nome")]
	[InlineData(10, "")]
	[InlineData(10, null)]
	public void Registrar_LivroInValido(int id, string nome)
	{
		var livroARegistrar  = new Livro{
			Id = id,
			Nome = nome,
		};

		var Valido = AcervoService.Livros.Registrar(livroARegistrar);

		Assert.False(Valido);
	}
}