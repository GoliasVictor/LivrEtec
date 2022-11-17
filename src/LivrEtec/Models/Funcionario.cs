using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec
{

	public class Funcionario : IFuncionario
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required, Key]
		public int Id { get; set; }
		public string Senha { get; set; } = null!;
		public string Login { get; set; } = null!;
		public string Nome { get; set; } = null!;

		public Funcionario()
		{
		}

		public Funcionario(int id, string senha, string login, string nome)
		{
			Id = id;
			Senha = senha;
			Login = login;
			Nome = nome;
		}
	}
}
