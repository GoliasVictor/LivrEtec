namespace LivrEtec.Servidor
{
    public interface IRepEmprestimo
    {
        Task<int> ObterQuantidadeLivrosEmprestadoAsync(int idLivro);
        Task<int> RegistrarAsync(Emprestimo emprestimo);
    }
}