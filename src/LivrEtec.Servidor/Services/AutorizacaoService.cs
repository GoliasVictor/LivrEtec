using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;
public sealed class AutorizacaoService : Service, IAutorizacaoService
{

	public AutorizacaoService(PacaContext bd, ILogger<AutorizacaoService> logger) 
		: base(bd, logger)
	{
	}

	public bool EhAutorizado(Usuario usuario, Permissao permissao)
	{
		return EhAutorizado(usuario.Id, permissao);
	}
	public bool EhAutorizado(int idUsuario, Permissao permissao)
	{
		if(permissao == null)
			throw new ArgumentNullException(nameof(permissao));
		if(!BD.Permissoes.Any((perm)=> perm.Id == permissao.Id))
			throw new ArgumentException(nameof(permissao));
			
		var autorizado = BD.Usuarios.Find(idUsuario)?.Cargo?.Permissoes?.Contains(permissao);
		return autorizado ?? false;
	}

	public Task<bool> EhAutorizadoAsync(Usuario usuario, Permissao permissao)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> EhAutorizadoAsync(int idUsuario, Permissao permissao)
	{
		if(permissao == null)
			throw new ArgumentNullException(nameof(permissao));
		if(!BD.Permissoes.Any((perm)=> perm.Id == permissao.Id))
			throw new ArgumentException(nameof(permissao));
			
		var autorizado = (await BD.Usuarios.FindAsync(idUsuario))?.Cargo?.Permissoes?.Contains(permissao);
		return autorizado ?? false;
	}

	public void ErroSeNaoAutorizado(Usuario usuario, Permissao permissao)
	{
		if (!EhAutorizado(usuario, permissao))
			throw new NaoAutorizadoException(usuario, permissao);
	}

	public async Task ErroSeNaoAutorizadoAsync(Usuario usuario, Permissao permissao)
	{
		if (await EhAutorizadoAsync(usuario, permissao) == false)
			throw new NaoAutorizadoException(usuario, permissao);
	}
}