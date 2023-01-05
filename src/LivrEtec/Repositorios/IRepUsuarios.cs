namespace LivrEtec.Servidor
{
    public interface IRepUsuarios
    {
		Task<bool> ExisteAsync(int id);
        Task<Usuario?> ObterAsync(int id);
    }
}