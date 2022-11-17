namespace LivrEtec
{
    public interface IPessoa
    {
        int Cd { get; set; }
        string Nome { get; set; }
        string? Telefone { get; set; }
    }
}