using Microsoft.Extensions.Logging;
namespace LivrEtec;
public interface IAutorazaoService {
	
	PacaContext BD { init;}
	ILogger? Logger {  init;} 
}

public class AutorizacaoService : IAutorazaoService
{
	public PacaContext BD { get;  init;}
	public ILogger? Logger { get; init;} 

	public AutorizacaoService(PacaContext bd, ILogger<AutorizacaoService> logger) 
	{
		BD = bd;
		Logger = logger;		
	}

	public bool EhAutorizado(Usuario usuario, Permissao permisao){
		if(usuario.Cargo == null){
			BD.Entry(usuario).Reference(u=> u.Cargo).Load();
		}
		var autorizado = BD.Usuarios.Find(usuario.Id)?.Cargo?.Permissoes?.Contains(permisao);
		return autorizado ?? false;
	}
}