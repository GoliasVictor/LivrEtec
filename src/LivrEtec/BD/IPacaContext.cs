using Microsoft.EntityFrameworkCore;

namespace LivrEtec
{
	public interface IPacaContext
	{
		DbSet<Livro> Livros { get; set; }
		DbSet<Autor> Autores { get; set; }
		DbSet<Tag> Tags { get; set; }
		DbSet<Aluno> Alunos { get; set; }
		DbSet<Emprestimo> Emprestimos { get; set; }
	}
}
