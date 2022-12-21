using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{

     
    public sealed class Emprestimo : IEmprestimo
    {
        public Emprestimo(int id, Aluno aluno, Livro livro, DateTime dataEmprestimo)
        {
            Id = id;
            Pessoa = aluno;
            Livro = livro;
            DataEmprestimo = dataEmprestimo;
        }

		public Emprestimo() { }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int Id { get; set; }


		[Required]
        public Pessoa Pessoa { get; set; } = null!;

        [Required]
        public Livro Livro { get; set; } = null!; 

        [Required]
        public Usuario UsuarioCriador = null!;

        public Usuario? UsuarioFechador = null!;

        [Required]
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
        public DateTime FimDataEmprestimo { get; set; }
        public string? Comentario { get; set; }

        public bool Fechado;
        [NotMapped]
        public bool Aberto {
            get => !Fechado;
            set => Fechado = !value;
        }
        public bool AtrasoJustificado;
        public string? ExplicaçãoAtraso;

        


    }
}