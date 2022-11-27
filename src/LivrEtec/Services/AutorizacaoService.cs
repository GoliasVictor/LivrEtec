using Microsoft.Extensions.Logging;
namespace LivrEtec;

public sealed class AutorizacaoService : IAutorizacaoService
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

	public void ErroSeNaoAutorizado(Usuario usuario, Permissao permissao)
	{
		if(!EhAutorizado(usuario, permissao))
			throw new NaoAutorizadoException(usuario, permissao);
	}
}