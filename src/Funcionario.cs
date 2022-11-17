using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec
{
    public class Funcionario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required, Key]
        public int Id;
        public string Senha = null!;
        public string Login = null!;
        public string Nome = null!;

        public Funcionario(int id, string senha, string login, string nome)
        {
            Id = id;
            Senha = senha;
            Login = login;
            Nome = nome;
        }
    }
}
