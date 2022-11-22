using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace LivrEtec
{

	public class PacaContext : DbContext, IPacaContext
	{
		public DbSet<Livro> Livros { get; set; } = null!;
		public DbSet<Autor> Autores { get; set; } = null!;
		public DbSet<Tag> Tags { get; set; } = null!;
		public DbSet<Aluno> Alunos { get; set; } = null!;
		public DbSet<Emprestimo> Emprestimos { get; set; } = null!;
		public ILoggerFactory? LoggerFactory { get; init; }
		public IConfiguracao Config;
		public PacaContext(IConfiguracao config, ILoggerFactory? loggerFactory = null)
		{
			_ = config ?? throw new NullReferenceException("Configuração não definida");
			Config = config;
			LoggerFactory = loggerFactory;

		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			//options.EnableSensitiveDataLogging(true);
			if (LoggerFactory != null)
				options.UseLoggerFactory(LoggerFactory);

			try
			{
				options.UseMySql(Config.StrConexaoMySQL, ServerVersion.AutoDetect(Config.StrConexaoMySQL));
			}
			catch (Exception ex)
			{
				Console.WriteLine("Falha ao usar banco de dados, sera utilizado banco de dados em memoria");
				Console.WriteLine(ex.Message);
				options.UseInMemoryDatabase("LivrEtecBD");
			}

		}
	}
}
