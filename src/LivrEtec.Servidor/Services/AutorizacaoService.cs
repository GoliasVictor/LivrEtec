using System.Data.Entity;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;
public sealed class AutorizacaoService : IAutorizacaoService
{

	readonly ILogger<AutorizacaoService> logger;
	readonly IRepUsuarios repUsuarios;
	public AutorizacaoService(IRepUsuarios repUsuarios, ILogger<AutorizacaoService> logger) 
	{
		this.repUsuarios = repUsuarios;
		this.logger = logger;
	}

	public async Task<bool> EhAutorizadoAsync(Usuario usuario, Permissao permissao)
	{
		if(usuario is null)
			return false;
		return await EhAutorizadoAsync(usuario.Id, permissao);
	}

	public async Task<bool> EhAutorizadoAsync(int idUsuario, Permissao permissao)
	{
		if(permissao == null)
			throw new ArgumentNullException(nameof(permissao));
		if(!Permissoes.TodasPermissoes.Contains(permissao))
			throw new ArgumentException(nameof(permissao));
		var usuario = await repUsuarios.ObterAsync(idUsuario) 
			?? throw new ArgumentException("Usuario Não Existe");
		return usuario.Cargo.Permissoes.Any( (p) => p.Id == permissao.Id);
	}
	public async Task ErroSeNaoAutorizadoAsync(Usuario usuario, Permissao permissao)
	{
		if (await EhAutorizadoAsync(usuario, permissao) == false)
			throw new NaoAutorizadoException(usuario, permissao);
	}
}