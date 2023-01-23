using System;
using System.Collections.Generic;
using LivrEtec.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.BD;
public sealed class PacaContext : DbContext
{
    public PacaContext(DbContextOptions<PacaContext> contextOptions) : base(contextOptions)
    {
    }
    public DbSet<Livro> Livros { get; set; } = null!;
    public DbSet<Autor> Autores { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Pessoa> Pessoas { get; set; } = null!;
    public DbSet<Emprestimo> Emprestimos { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Cargo> Cargos { get; set; } = null!;
    public DbSet<Permissao> Permissoes { get; set; } = null!;
}
