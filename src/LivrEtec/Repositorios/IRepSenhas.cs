namespace LivrEtec.Repositorios;

public interface IRepSenhas
{
    Task Editar(int IdUsuario, string hash );
    Task<string> Obter(int IdUsuario);
}
