namespace LivrEtec.Repositorios;

public interface IRepPessoas
{
    Task<Pessoa?> ObterObter(int id);
}