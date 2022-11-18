using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{

     
    public class Emprestimo : IEmprestimo
    {
        public Emprestimo(int id, Aluno aluno, Livro livro, DateTime dataEmprestimo)
        {
            Id = id;
            Aluno = aluno;
            Livro = livro;
            DataEmprestimo = dataEmprestimo;
        }

		public Emprestimo() { }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int Id { get; set; }


		[Required]
        public Aluno Aluno { get; set; } = null!;

        [Required]
        public Livro Livro { get; set; } = null!; 

        [Required]
        public Funcionario Funcionario = null!;

        [Required]
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
        public DateTime FimDataEmprestimo { get; set; }
        public string? Comentario { get; set; }

        public bool AtrasoJustificado;
        public string? ExplicaçãoAtraso;

        


    }
}