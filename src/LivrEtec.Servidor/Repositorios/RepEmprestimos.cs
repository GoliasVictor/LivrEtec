using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec.Servidor
{
    public class RepEmprestimos : Repositorio, IRepEmprestimo
    {
        public RepEmprestimos(AcervoService acervoService) : base(acervoService)
        {

        }
        public async Task<int> RegistrarAsync(Emprestimo emprestimo)
        {
            await BD.Emprestimos.AddAsync(emprestimo);
            BD.SaveChanges();
            return emprestimo.Id;

        }
        public async Task<int> ObterQuantidadeLivrosEmprestadoAsync(int idLivro)
        {
            return await BD.Emprestimos.CountAsync((e) => e.Livro.Id == idLivro && !e.Fechado);
        }
    }
}
