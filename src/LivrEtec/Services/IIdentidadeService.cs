namespace LivrEtec.Services;

public interface IIdentidadeService
{
    Usuario? Usuario { get; set; }
    bool EstaAutenticado { get; set; }
    Task Login(string login, string senha, bool SenhaHash);
    Task CarregarUsuario();
    Task<bool> EhAutorizado(Permissao permissao);
    Task ErroSeNaoAutorizado(Permissao permissao);

}
