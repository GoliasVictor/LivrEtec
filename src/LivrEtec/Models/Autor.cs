using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{
    public class Autor : IAutor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int cd { get; set; }
        [Required]
        public string Nome { get; set;} = null!;
        public List<Livro> Livros {get;set;} =  new();
        public Autor(){}
        public Autor(int cd = default, string nome = null!)
        {
            this.cd = cd;
            this.Nome = nome;
        }
        public Autor(int cd )
        {
            this.cd = cd;
        }
    }
}