using System.ComponentModel.DataAnnotations;

namespace LivrEtec.Models
{
    public class Aluno : Pessoa, IEquatable<Aluno?>
    {
        public Aluno() { }
        public Aluno(string nome, string telefone, string rm) : base(nome, telefone)
        {
            RM = rm;
        }

        [Required]
        public string RM { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            return Equals(obj as Aluno);
        }

        public bool Equals(Aluno? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Id == other.Id &&
                   Nome == other.Nome &&
                   Telefone == other.Telefone &&
                   RM == other.RM;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, Nome, Telefone, RM);
        }

        public static bool operator ==(Aluno? left, Aluno? right)
        {
            return EqualityComparer<Aluno>.Default.Equals(left, right);
        }

        public static bool operator !=(Aluno? left, Aluno? right)
        {
            return !(left == right);
        }
    }
}