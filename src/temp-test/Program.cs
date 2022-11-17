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
  
var Autores = Acervo.Autores.Todos().ToList();


var AutorAleatorio =  () => Autores[rnd.Next(Autores.Count())];
Acervo.Livros.Registrar(new Livro(){
	Nome = $"Carlos {rnd.NextDouble()}",
	Arquivado =  false,
	Autores =  new List<Autor>(){AutorAleatorio()},
	Tags = {new Tag(nome:rnd.NextDouble().ToString())},
	Descricao = "Socorro"
});
var Livros = Acervo.Livros.Buscar("", "", new List<Tag>{});
Console.WriteLine(String.Join(",", Livros.Select(l => l.Nome)));