namespace LivrEtec.Servidor
{
    public interface IRepUsuarios
    {
        Task<Usuario?> ObterAsync(int id);
    }
}