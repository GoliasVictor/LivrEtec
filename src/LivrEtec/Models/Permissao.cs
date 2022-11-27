using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec;
public class Permissao 
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
	[Required, Key]
	public int Id { get; set; }
	[Required]
	public string Nome { get; set; } = null!;
	[Required]
	public string Descricao { get; set; } = null!;
	public List<Permissao> PermissoesDependete { get; set; } = new List<Permissao>();
	[Required]
	public List<Cargo> Cargos { get; set; } =  null!;
}