namespace LivrEtec.Servidor
{
    public interface IRepUsuarios
    {
		Task<bool> Existe(int id);
        Task<Usuario?> Obter(int id);
    }
}