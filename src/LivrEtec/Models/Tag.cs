using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{
    public sealed class Tag : IComparable<Tag>, IEquatable<Tag?>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
        public List<Livro> Livros {get;set;} =  new();
        public Tag(int id = default, string nome = null!)
        {
            this.Id = id;
            this.Nome = nome;
        }
        public Tag(int id)
        {
            this.Id = id;
        }
        public Tag(string nome)
        {
            this.Nome = nome;
        }
        public Tag()
		{
		}

		public int CompareTo(Tag? other)
		{
            _ = other ?? throw new NullReferenceException();
            return Id.CompareTo(other.Id);
		}

        public override bool Equals(object? obj)
        {
            return Equals(obj as Tag);
        }

        public bool Equals(Tag? other)
        {
            return other is not null &&
                   Id == other.Id &&
                   Nome == other.Nome;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nome);
        }

        public static bool operator ==(Tag? left, Tag? right)
        {
            return EqualityComparer<Tag>.Default.Equals(left, right);
        }

        public static bool operator !=(Tag? left, Tag? right)
        {
            return !(left == right);
        }
    }
}