namespace LivrEtec;

public interface IAutorizacaoService
{
	Task<bool> EhAutorizado(Usuario usuario, Permissao permissao);
	Task<bool> EhAutorizadoAsync(int idUsuario, Permissao permissao);
	Task ErroSeNaoAutorizadoAsync(Usuario usuario, Permissao permissao);
}
