using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec.Models
{
    public sealed class Autor : IComparable<Autor>, IEquatable<Autor?>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Required, InteiroPositivo(nameof(Id))]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; } = null!;
        public List<Livro> Livros { get; set; } = new();
        public Autor() { }
        public Autor(int id = default, string nome = null!)
        {
            Id = id;
            Nome = nome;
        }
        public Autor(int id)
        {
            Id = id;
        }

        public int CompareTo(Autor? other)
        {
            _ = other ?? throw new NullReferenceException();
            return Id.CompareTo(other.Id);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Autor);
        }

        public bool Equals(Autor? other)
        {
            return other is not null &&
                   Id == other.Id &&
                   Nome == other.Nome;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nome);
        }

        public static bool operator ==(Autor? left, Autor? right)
        {
            return EqualityComparer<Autor>.Default.Equals(left, right);
        }

        public static bool operator !=(Autor? left, Autor? right)
        {
            return !(left == right);
        }
    }
}