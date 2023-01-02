namespace LivrEtec
{
    public interface IRepPessoas
    {
        Task<Pessoa?> ObterAsync(int id);
    }
}