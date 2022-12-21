using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec
{
    public interface IEmprestimoService
    {
        Task<int> AbrirAsync(int idPessoa, int idlivro);
        Task RegistrarDevolucaoAsync(int idEmprestimo);
        Task RegistrarPerdaAsync(int idEmprestimo, string justificativa);
        Task ProrrogarAsnc(int idEmprestimo, DateTime novaData);
        Task<IEnumerable<Emprestimo>> BuscarAsync(int idPessoa, bool aberto, bool atrasado );

    }
    
}
