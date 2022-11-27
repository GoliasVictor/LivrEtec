using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec
{

	public class Usuario : IUsuario
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required, Key]
		public int Id { get; set; }
		[Required]
		public string Senha { get; set; } = null!;
		[Required]
		public string Login { get; set; } = null!;
		[Required]
		public string Nome { get; set; } = null!;
		[Required]
		public Cargo Cargo { get; set; } = null!;

		public Usuario()
		{
		}

		public Usuario(int id, string senha, string login, string nome, Cargo cargo)
		{
			Id = id;
			Senha = senha;
			Login = login;
			Nome = nome;
			Cargo = cargo;
		}
	}
}
