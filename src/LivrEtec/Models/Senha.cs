using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec.Models;

public sealed class Senha
{
	public Senha(int idUsuario, string hash)
	{
		IdUsuario = idUsuario;
		Hash = hash;
	}

	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Key, Required, InteiroPositivo(nameof(IdUsuario))]
	public int IdUsuario { get; set; }
	[ForeignKey(nameof(IdUsuario))]
	public Usuario Usuario { get; set; } = null!;
	[Required]
	public string Hash { get; set; } = null!;

}