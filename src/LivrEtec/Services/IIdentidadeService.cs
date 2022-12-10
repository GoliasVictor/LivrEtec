namespace LivrEtec;

public interface IIdentidadeService
{
	int IdUsuario { get; }
	Usuario? Usuario { get; }
	bool EstaAutenticado { get; }

	void AutenticarUsuario(string senha);
	Task AutenticarUsuarioAsync(string senha);
	void DefinirUsuario(int idUsuario);
	Task DefinirUsuarioAsync(int idUsuario);
	bool EhAutorizado(Permissao permissao);
	Task<bool> EhAutorizadoAsync(Permissao permissao);
	void ErroSeNaoAutorizado(Permissao permissao);
	Task ErroSeNaoAutorizadoAsync(Permissao permissao);

}
