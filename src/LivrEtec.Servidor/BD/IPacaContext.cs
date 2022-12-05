using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec
{
	public interface IPacaContext
	{
		DbSet<Livro> Livros { get; set; }
		DbSet<Autor> Autores { get; set; }
		DbSet<Tag> Tags { get; set; }
		DbSet<Pessoa> Pessoas { get; set; }
		DbSet<Emprestimo> Emprestimos { get; set; }
		DbSet<Usuario> Usuarios { get; set; }
		DbSet<Cargo> Cargos { get; set; }
		DbSet<Permissao> Permissoes { get; set; }
	}
}
