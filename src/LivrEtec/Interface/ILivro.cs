namespace LivrEtec
{
    public interface ILivro
    {
        bool Arquivado { get; set; }
        Autor[] Autores { get; set; }
        int cd { get; set; }
        string Nome { get; set; }
        Tag[] Tags { get; set; }
    }
}