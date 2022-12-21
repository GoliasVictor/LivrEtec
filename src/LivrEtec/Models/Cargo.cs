using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{
    public sealed class Cargo :  IComparable<Cargo>
    {
		public Cargo()
		{
		}

		public Cargo(int id, string nome, List<Permissao> permissoes)
		{
			Id = id;
			Nome = nome;
			Permissoes = permissoes;
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; } = null!;
        public List<Permissao> Permissoes {get;set;} = new();
		
		public int CompareTo(Cargo? other)
		{
            _ = other ?? throw new NullReferenceException();
            return Id.CompareTo(other.Id);
		}
	}
}