using System.ComponentModel.DataAnnotations;

namespace LivrEtec
{

     
    public class Emprestimo : IEmprestimo
    {
        public Emprestimo(Aluno aluno, Livro livro, DateTime dataEmprestimo)
        {
            Aluno = aluno;
            Livro = livro;
            DataEmprestimo = dataEmprestimo;
        }
        [Required]
        public Aluno Aluno { get; set; }

        [Required]
        public Livro Livro { get; set; }

        [Required]
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
        public DateTime FimDataEmprestimo { get; set; }
        public string? Comentario { get; set; }

        public bool AtrasoJustificado;
        public string? ExplicaçãoAtraso;


    }
}