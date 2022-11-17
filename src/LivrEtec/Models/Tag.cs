using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{
    public class Tag : ITag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int Cd { get; set; }
        [Required]
        public string? Nome { get; set; }
        public List<Livro> Livros {get;set;} =  new();
        public Tag(int cd = default, string nome = null!)
        {
            this.Cd = cd;
            this.Nome = nome;
        }
        public Tag(int cd)
        {
            this.Cd = cd;
        }
        public Tag(string nome)
        {
            this.Nome = nome;
        }
        public Tag()
		{
		}

 
	}
}