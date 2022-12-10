namespace LivrEtec;

public interface IAutorizacaoService
{
	bool EhAutorizado(Usuario usuario, Permissao permissao);
	Task<bool> EhAutorizadoAsync(Usuario usuario, Permissao permissao);
	bool EhAutorizado(int idUsuario, Permissao permissao);
	Task<bool> EhAutorizadoAsync(int idUsuario, Permissao permissao);
	void ErroSeNaoAutorizado(Usuario usuario, Permissao permissao);
	Task ErroSeNaoAutorizadoAsync(Usuario usuario, Permissao permissao);
}
