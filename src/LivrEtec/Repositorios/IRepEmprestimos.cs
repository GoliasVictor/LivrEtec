namespace LivrEtec.Servidor
{
    public interface IRepEmprestimos
    {
        Task<int> ObterQuantidadeLivrosEmprestadoAsync(int idLivro);
        Task<int> RegistrarAsync(Emprestimo emprestimo);
        Task<Emprestimo?> ObterAsync(int idEmprestimo);
        Task<IEnumerable<Emprestimo>> BuscarAsync(ParamBuscaEmprestimo parametros);
        Task FecharAsync(ParamFecharEmprestimo parametros);
        Task EditarFimData(int idLivro, DateTime NovaData);
        Task Excluir(int idEmprestimo);
    }
}