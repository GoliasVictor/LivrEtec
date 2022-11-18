using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec
{

	public class PacaContext : DbContext, IPacaContext
	{
		public DbSet<Livro> Livros { get; set; } = null!;
		public DbSet<Autor> Autores { get; set; } = null!;
		public DbSet<Tag> Tags { get; set; } = null!;
		public DbSet<Aluno> Alunos { get; set; } = null!;
		public DbSet<Emprestimo> Emprestimos { get; set; } = null!;
		public ILoggerFactory? LoggerFactory { get;init; }
		public PacaContext(ILoggerFactory? loggerFactory = null)
		{ 
			LoggerFactory = loggerFactory;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			//options.EnableSensitiveDataLogging(true);
			if(LoggerFactory != null)
				options.UseLoggerFactory(LoggerFactory);
			var stringConn = $"server=localhost;database=LivrEtecBD;user=LivrEtecServe;password=LivrEtecSenha";
			options.UseMySql(stringConn, ServerVersion.AutoDetect(stringConn));

		}
	}
}
