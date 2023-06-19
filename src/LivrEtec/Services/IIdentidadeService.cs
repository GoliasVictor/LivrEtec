using System.Diagnostics.CodeAnalysis;

namespace LivrEtec.Services;

public interface IIdentidadeService
{
    
    int? IdUsuario { get; set; }
    bool EstaAutenticado { get; set; }
    Task Login(string login, string senha, bool SenhaHash);
    Task CarregarUsuario();
    Task<Usuario?> ObterUsuario();
    Task<bool> EhAutorizado(Permissao permissao);
    [MemberNotNull(nameof(IdUsuario))]
    Task ErroSeNaoAutorizado(Permissao permissao);

}
