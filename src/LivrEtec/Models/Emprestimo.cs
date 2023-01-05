using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{

     
    public sealed class Emprestimo 
    {
        public Emprestimo(int id, Aluno aluno, Livro livro, DateTime dataEmprestimo)
        {
            Id = id;
            Pessoa = aluno;
            Livro = livro;
            DataEmprestimo = dataEmprestimo;
        }
        public Emprestimo Clone(){
            return new Emprestimo(){
                Id =  Id,
                Livro = Livro,
                Pessoa = Pessoa,
                UsuarioCriador = UsuarioCriador,
                Fechado = Fechado,
                DataEmprestimo =  DataEmprestimo,
                DataFechamento = DataFechamento,
                FimDataEmprestimo = FimDataEmprestimo,
                AtrasoJustificado = AtrasoJustificado,
                Comentario =  Comentario,
                UsuarioFechador = UsuarioFechador,
                Devolvido = Devolvido,
                ExplicacaoAtraso = ExplicacaoAtraso,
            };
        }
		public Emprestimo() { }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Required, InteiroPositivo(nameof(Id))]
        public int Id { get; set; }


		[Required]
        public Pessoa Pessoa { get; set; } = null!;

        [Required]
        public Livro Livro { get; set; } = null!; 

        [Required]
        public Usuario UsuarioCriador {get;set;} = null!;

        public Usuario? UsuarioFechador {get;set;} = null!;

        [Required]
        public DateTime DataEmprestimo { get; set; }
        [Required]
        public DateTime FimDataEmprestimo { get; set; }
        public DateTime? DataFechamento { get; set; }
        public string? Comentario { get; set; }

        public bool Fechado { get; set; }
        public bool? Devolvido { get; set; }
        public bool? AtrasoJustificado { get; set; }
        public string? ExplicacaoAtraso {get;set;}

        


    }
}