using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{
    public sealed class Autor : IAutor, IComparable<Autor>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set;} = null!;
        public List<Livro> Livros {get;set;} =  new();
        public Autor(){}
        public Autor(int id = default, string nome = null!)
        {
            this.Id = id;
            this.Nome = nome;
        }
        public Autor(int id )
        {
            this.Id = id;
        }

		public int CompareTo(Autor? other)
		{
            _ = other ?? throw new NullReferenceException();
            return this.Id.CompareTo(other.Id);
		}
        public override bool Equals(object? obj)
		{
            return obj switch {
                Autor autor => autor.Id ==  this.Id,
                _ => base.Equals(obj)
            };
		}

        public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
 
	}
}