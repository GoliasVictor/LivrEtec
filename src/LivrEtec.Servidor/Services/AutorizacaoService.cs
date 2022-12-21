using System.Data.Entity;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;
public sealed class AutorizacaoService : Service, IAutorizacaoService
{

	public AutorizacaoService(PacaContext bd, ILogger<AutorizacaoService> logger) 
		: base(bd, logger)
	{
	}

	public Task<bool> EhAutorizadoAsync(Usuario usuario, Permissao permissao)
	{
		return EhAutorizadoAsync(usuario.Id, permissao);
	}

	public async Task<bool> EhAutorizadoAsync(int idUsuario, Permissao permissao)
	{
		bool autorizado =  false;
		await Task.Run(()=> {
			if(permissao == null)
				throw new ArgumentNullException(nameof(permissao));
			if(!BD.Permissoes.Any((perm)=> perm.Id == permissao.Id))
				throw new ArgumentException(nameof(permissao));
			
			var usuario =  BD.Usuarios.SingleOrDefault( u => u.Id ==  idUsuario);

			if(!BD.Permissoes.Any(u=> u.Id == idUsuario))
				throw new ArgumentException(nameof(Usuario));
			autorizado = BD.Usuarios.Where( u => u.Id ==  idUsuario).Any(u => u.Cargo.Permissoes.Contains(permissao));
		});
		
		return autorizado;
	}
	public async Task ErroSeNaoAutorizadoAsync(Usuario usuario, Permissao permissao)
	{
		if (await EhAutorizadoAsync(usuario, permissao) == false)
			throw new NaoAutorizadoException(usuario, permissao);
	}
}