using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LivrEtec
{

	public class PacaContext : DbContext, IPacaContext
	{
		public DbSet<Livro> Livros { get; set; } = null!;
		public DbSet<Autor> Autores { get; set; } = null!;
		public DbSet<Tag> Tags { get; set; } = null!;
		public DbSet<Aluno> Alunos { get; set; } = null!;
		public DbSet<Emprestimo> Emprestimos { get; set; } = null!;

		public PacaContext()
		{ 
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			//options.EnableSensitiveDataLogging(true);
			options.LogTo((s => Console.WriteLine(s)), Microsoft.Extensions.Logging.LogLevel.Information);
			var stringConn = $"server=localhost;database=LivrEtecBD;user=LivrEtecServe;password=LivrEtecSenha";
			options.UseMySql(stringConn, ServerVersion.AutoDetect(stringConn));

		}
	}
}
