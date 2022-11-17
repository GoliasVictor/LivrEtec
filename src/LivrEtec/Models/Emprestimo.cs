using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{

     
    public class Emprestimo : IEmprestimo
    {
        public Emprestimo(int cd, Aluno aluno, Livro livro, DateTime dataEmprestimo)
        {
            Cd = cd;
            Aluno = aluno;
            Livro = livro;
            DataEmprestimo = dataEmprestimo;
        }

		public Emprestimo()
		{
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int Cd { get; set; }


		[Required]
        public Aluno? Aluno { get; set; }

        [Required]
        public Livro? Livro { get; set; }

        [Required]
        public Funcionario Funcionario;

        [Required]
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
        public DateTime FimDataEmprestimo { get; set; }
        public string? Comentario { get; set; }

        public bool AtrasoJustificado;
        public string? ExplicaçãoAtraso;

        


    }
}