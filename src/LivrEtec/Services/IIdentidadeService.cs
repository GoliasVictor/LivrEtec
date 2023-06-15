namespace LivrEtec.Services;

public interface IIdentidadeService
{
    Usuario? Usuario { get; set; }
    bool EstaAutenticado { get; set; }
    Task AutenticarEDefinirUsuario(string login, string senha);
    Task CarregarUsuario();
    Task<bool> EhAutorizado(Permissao permissao);
    Task ErroSeNaoAutorizado(Permissao permissao);

}
