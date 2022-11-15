using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{
    public class Livro : ILivro
    {
        public Livro(){}
        public Livro(int cd, string nome, string descricao, List<Tag> tags, List<Autor> autores, bool arquivado = false)
        {
            this.cd = cd;
            Nome = nome;
            Descricao = descricao;
            Tags = tags;
            Autores = autores;
            Arquivado = arquivado;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int cd { get; set; }

        [Required]
        public string Nome { get; set; }
        public string? Descricao { get; set; } 
        public List<Tag> Tags { get; set; } =  new List<Tag>();
        public List<Autor> Autores { get; set; } =  new List<Autor>();

        [Required]
        public bool Arquivado { get; set; }
    }
}