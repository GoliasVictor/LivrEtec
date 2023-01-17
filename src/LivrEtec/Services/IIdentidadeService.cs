using System.Diagnostics.CodeAnalysis;

namespace LivrEtec;

public interface IIdentidadeService
{
	int IdUsuario { get; }
	Usuario? Usuario { get; }
	bool EstaAutenticado { get; }
	Task AutenticarUsuario(string senha);
	Task AutenticarUsuario();
	Task DefinirUsuario(int idUsuario);
	Task<bool> EhAutorizado(Permissao permissao);
	Task ErroSeNaoAutorizado(Permissao permissao);

}
