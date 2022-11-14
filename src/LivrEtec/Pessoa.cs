using System.ComponentModel.DataAnnotations;

namespace LivrEtec
{
    public class Pessoa : IPessoa
    {
        public Pessoa(string nome, string telefone)
        {
            Nome = nome;
            Telefone = telefone;
        }

        [Required, Key]
        public int Cd { get; set; }

        [Required]
        public string Nome { get; set; }

        [MaxLength(14)]
        public string Telefone { get; set; }
    }
    public class Aluno : Pessoa
    {
        public Aluno(string nome, string telefone, string rm) : base(nome, telefone)
        {
            RM = rm;
        }

        [Required,Key]
        public string RM { get; set; }
    }
}