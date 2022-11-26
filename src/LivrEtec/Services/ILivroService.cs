namespace LivrEtec
{
    public interface ILivroService
    {
        IQueryable<Livro> Buscar(string nome, string nomeAutor, IEnumerable<Tag>? tags = null);
        Livro? Get(int id);
        bool Registrar(Livro livro);
    }
}