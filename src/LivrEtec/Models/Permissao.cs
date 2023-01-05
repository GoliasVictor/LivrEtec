using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace LivrEtec;
[Display()]
[DebuggerDisplay("{Id}#{Nome}}")]
public sealed class Permissao : IComparable<Permissao>
{
	public Permissao()
	{
	}

	public Permissao(int id, string nome, string descricao, List<Permissao>? permissoesDependete = null)
	{
		Id = id;
		Nome = nome;
		Descricao = descricao;
		if(permissoesDependete != null)
			PermissoesDependete =  permissoesDependete;
	}

	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Required, InteiroPositivo(nameof(Id))]
	public int Id { get; set; }
	[Required]
	public string Nome { get; set; } = null!;
	[Required]
	public string Descricao { get; set; } = null!;
	public List<Permissao> PermissoesDependete { get; set; } = new List<Permissao>();
	[Required]
	public List<Cargo> Cargos { get; set; } =  null!;

	public int CompareTo(Permissao? other)
	{
		return Id.CompareTo(other?.Id);
	}

 
}