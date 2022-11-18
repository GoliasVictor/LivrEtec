namespace LivrEtec
{
    public interface IPessoa
    {
        int Id { get; set; }
        string Nome { get; set; }
        string? Telefone { get; set; }
    }
}