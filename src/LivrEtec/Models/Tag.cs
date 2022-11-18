using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec
{
    public class Tag : ITag, IComparable<Tag>
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
            return this.Id.CompareTo(other.Id);
		}
	}
}