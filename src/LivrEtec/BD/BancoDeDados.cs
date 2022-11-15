using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LivrEtec
{
	public class PacaContext : DbContext
	{
		public DbSet<Livro> Livros { get; set; }
		public DbSet<Autor> Autores { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Aluno> Alunos { get; set; }
		public DbSet<Emprestimo> Emprestimos { get; set; }

		public string DbPath { get; }

		public PacaContext()
		{
			Database.Migrate();

		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
            var stringConn = $"server=localhost;database=LivrEtecBD;user=LivrEtecServe;password=LivrEtecSenha";
            options.UseMySql(stringConn, ServerVersion.AutoDetect(stringConn));

		}
	}
}
