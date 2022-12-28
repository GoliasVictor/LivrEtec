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
        Task<IEnumerable<Emprestimo>> BuscarAsync(ParamBuscaEmprestimo parametros );
        Task ProrrogarAsnc(int idEmprestimo, DateTime novaData);
        Task DevolverAsync(int idEmprestimo, bool? AtrasoJustificado = null, string? ExplicacaoAtraso=null);
        Task RegistrarPerdaAsync(int idEmprestimo);
	}
    
}
