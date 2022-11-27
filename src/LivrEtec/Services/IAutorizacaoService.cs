using Microsoft.Extensions.Logging;
namespace LivrEtec;

public interface IAutorizacaoService {
	
	PacaContext BD { init;}
	ILogger? Logger {  init;}
	bool EhAutorizado(Usuario usuario, Permissao permisao);
}
