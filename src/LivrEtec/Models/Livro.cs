using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{
    public sealed class Livro : ILivro, IEquatable<Livro?>
    {
        public Livro(){}
        public Livro(string nome, string descricao, List<Tag>? tags =  default, List<Autor>? autores = default, bool arquivado = false)
        {
            Id =  default;
            Nome = nome;
            Descricao = descricao;
            Tags = tags ?? new() ;
            Autores = autores ?? new();
            Arquivado = arquivado;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = null!;
        public string? Descricao { get; set; } 
        public int Quantidade { get; set; }
        public List<Tag> Tags { get; set; } = new ();
        public List<Autor> Autores { get; set; } = new();
        [Required]
        public bool Arquivado { get; set; }
 
        public Livro Clone(){
            return new Livro(){
                Id =  Id,
                Nome = Nome,
                Arquivado =  Arquivado,
                Descricao =  Descricao,
                Quantidade = Quantidade,
                Autores = Autores.Select(a=>a).ToList(),
                Tags = Tags.Select(t=>t).ToList(),
            };
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Livro);
        }

        public bool Equals(Livro? other)
        {
            return other is not null &&
                   Id == other.Id &&
                   Nome == other.Nome &&
                   Descricao == other.Descricao &&
                   Quantidade ==  other.Quantidade &&
                   EqualityComparer<ISet<Tag>>.Default.Equals(Tags.ToHashSet(), other.Tags.ToHashSet()) &&
                   EqualityComparer<ISet<Autor>>.Default.Equals(Autores.ToHashSet(), other.Autores.ToHashSet()) &&
                   Arquivado == other.Arquivado;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nome, Descricao, Tags, Autores, Arquivado, Quantidade);
        }

        public static bool operator ==(Livro? left, Livro? right)
        {
            return EqualityComparer<Livro>.Default.Equals(left, right);
        }

        public static bool operator !=(Livro? left, Livro? right)
        {
            return !(left == right);
        }
    }
}