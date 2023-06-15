namespace LivrEtec.Repositorios;

public interface IRepUsuarios
{
    Task<bool> Existe(int id);
    Task<Usuario?> Obter(int id);
    Task<int?> ObterId(string login);
}