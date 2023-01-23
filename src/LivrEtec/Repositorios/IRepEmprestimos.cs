using LivrEtec.Models;

namespace LivrEtec.Repositorios
{
    public interface IRepEmprestimos
    {
        Task<int> ObterQuantidadeLivrosEmprestado(int idLivro);
        Task<int> Registrar(Emprestimo emprestimo);
        Task<Emprestimo?> Obter(int idEmprestimo);
        Task<IEnumerable<Emprestimo>> Buscar(ParamBuscaEmprestimo parametros);
        Task Fechar(ParamFecharEmprestimo parametros);
        Task EditarFimData(int idLivro, DateTime NovaData);
        Task Excluir(int idEmprestimo);
    }
}