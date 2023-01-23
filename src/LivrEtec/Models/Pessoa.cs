using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec.Models
{
    public class Pessoa : IEquatable<Pessoa?>
    {
        public Pessoa() { }
        public Pessoa(string nome, string telefone)
        {
            Nome = nome;
            Telefone = telefone;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Required, InteiroPositivo(nameof(Id))]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = null!;

        [MaxLength(14)]
        public string? Telefone { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Pessoa);
        }

        public bool Equals(Pessoa? other)
        {
            return other is not null &&
                   Id == other.Id &&
                   Nome == other.Nome &&
                   Telefone == other.Telefone;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nome, Telefone);
        }

        public static bool operator ==(Pessoa? left, Pessoa? right)
        {
            return EqualityComparer<Pessoa>.Default.Equals(left, right);
        }

        public static bool operator !=(Pessoa? left, Pessoa? right)
        {
            return !(left == right);
        }

    }
}