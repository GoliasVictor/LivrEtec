using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec
{
    public interface IEmprestimoService
    {
        Task<int> Abrir(int idPessoa, int idlivro);
        Task<IEnumerable<Emprestimo>> Buscar(ParamBuscaEmprestimo parametros );
        Task Prorrogar(int idEmprestimo, DateTime novaData);
        Task Devolver(int idEmprestimo, bool? AtrasoJustificado = null, string? ExplicacaoAtraso=null);
        Task RegistrarPerda(int idEmprestimo);
        Task Excluir(int idEmprestimo);
	}
    
}
