using System;
using System.Linq;
using LivrEtec;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using var db = new PacaContext();

var rnd = new Random();

ILogger Logger = LoggerFactory.Create((lb)=> { 
	lb.AddConsole();
	lb.AddFilter((_,_, logLevel)=> logLevel > LogLevel.Information);
}).CreateLogger<AcervoService>();
var Acervo = new AcervoService(db,Logger);
Acervo.BD.Database.EnsureDeleted();
Acervo.BD.Database.EnsureCreated();
Acervo.Autores.Registrar(
	new Autor(nome: "Carlito"+rnd.Next(1000))
);
  
T ElementoAleatorio<T>(IEnumerable<T> array) {
	return array.ElementAt(rnd.Next(array.Count()));
}
var Autores = Acervo.Autores.Todos().ToList();

var Tags = new[]{
	new Tag("Aventura"),
	new Tag("Fantasia"),
	new Tag("Arroz"),
	new Tag("SOS"),
	new Tag("Socorro")
};
Acervo.BD.Tags.AddRange(Tags); 
for (int i = 0; i < 10; i++)
	Acervo.Livros.Registrar(new Livro(){
		Nome = $"Carlos {rnd.NextDouble()}",
		Arquivado =  false,
		Autores =  new List<Autor>(){ ElementoAleatorio(Autores)},
		Tags = { ElementoAleatorio(Tags), ElementoAleatorio(Tags), ElementoAleatorio(Tags) },
		Descricao = "Socorro"
	});

var Livros = Acervo.Livros.Buscar("", "", new List<Tag>{Tags[0], Tags[1]});
Console.WriteLine(String.Join(",", Livros.Select(l => l.Nome)));