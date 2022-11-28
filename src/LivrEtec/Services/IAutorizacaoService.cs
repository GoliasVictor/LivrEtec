using Microsoft.Extensions.Logging;
namespace LivrEtec;

public interface IAutorizacaoService
{
	bool EhAutorizado(Usuario usuario, Permissao permissao);
	bool EhAutorizado(int idUsuario, Permissao permissao);
	void ErroSeNaoAutorizado(Usuario usuario, Permissao permissao);
}
