using System;
using System.Linq;
using LivrEtec;

using var db = new PacaContext();
var Acervo = new AcervoService(db);

var Livros = Acervo.BuscarLivro("d", new List<Tag>{new Tag(1),new Tag(2)});
Console.WriteLine(String.Join(",", Livros.Select(l => l.Nome)));