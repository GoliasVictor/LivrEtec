using System.Diagnostics.CodeAnalysis;

namespace LivrEtec;

public interface IIdentidadeService
{
	int IdUsuario { get; }
	Usuario? Usuario { get; }
	bool EstaAutenticado { get; }
	Task AutenticarUsuarioAsync(string senha);
	Task AutenticarUsuarioAsync();
	Task DefinirUsuarioAsync(int idUsuario);
	Task<bool> EhAutorizadoAsync(Permissao permissao);
	Task ErroSeNaoAutorizadoAsync(Permissao permissao);

}
