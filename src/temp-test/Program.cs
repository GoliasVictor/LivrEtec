using System;
using System.Linq;
using LivrEtec;

using var db = new PacaContext();

// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

// Created
Console.WriteLine("Inserting a new blog");
db.Add(new Livro{Nome="carlete", Descricao = "dois"});
db.SaveChanges();

Console.WriteLine("Querying for a blog");
var blog = db.Livros
    .OrderBy(b => b.cd)
    .First();
// Update
Console.WriteLine("Updating the blog and adding a post");
blog.Nome = "https://devblogs.microsoft.com/dotnet";
blog.Tags.Add(new Tag{Nome = "Carlos"});
db.SaveChanges();

// Delete
Console.WriteLine("Delete the blog");
db.Remove(blog);
db.SaveChanges();