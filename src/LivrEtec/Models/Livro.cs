using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{
    public class Livro : ILivro
    {
        public Livro(){}
        public Livro(string nome, string descricao, List<Tag>? tags =  default, List<Autor>? autores = default, bool arquivado = false)
        {
            cd =  default;
            Nome = nome;
            Descricao = descricao;
            Tags = tags ?? new() ;
            Autores = autores ?? new();
            Arquivado = arquivado;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int cd { get; set; }

        [Required]
        public string Nome { get; set; } = null!;
        public string? Descricao { get; set; } 
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Autor> Autores { get; set; } = new List<Autor>();
        [Required]
        public bool Arquivado { get; set; }
    }
}