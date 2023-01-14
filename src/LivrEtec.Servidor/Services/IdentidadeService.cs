using System.Data.Entity;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public class IdentidadeService : IIdentidadeService
{
	ILogger<IdentidadeService>? logger;
	IRepUsuarios repUsuarios;
	public IdentidadeService( 
		IRepUsuarios repUsuarios,
		IAutorizacaoService autorizacaoService,
		IAutenticacaoService autenticacaoService,
	 	ILogger<IdentidadeService>? logger
	)
	{
		this.repUsuarios = repUsuarios;
		this.logger = logger;
		AutorizacaoService = autorizacaoService;
		AutenticacaoService = autenticacaoService;
	}
	IAutorizacaoService AutorizacaoService { get; set; }
	IAutenticacaoService AutenticacaoService { get; set; }
	public int IdUsuario { get; private set; }
	public Usuario? Usuario { get; private set; }
	public bool EstaAutenticado { get; private set; }

	public async Task DefinirUsuarioAsync(int idUsuario)
	{
		if (false == await repUsuarios.ExisteAsync(idUsuario))
			throw new ArgumentException("Usuario não existe");
		EstaAutenticado = false;
		IdUsuario = idUsuario;
		
	}

 	public async Task AutenticarUsuarioAsync(string senha)
	{
		EstaAutenticado = await AutenticacaoService.EhAutenticoAsync(IdUsuario, senha);
		if (EstaAutenticado)
			Usuario = await repUsuarios.ObterAsync(IdUsuario);
		
	}
	public async Task AutenticarUsuarioAsync()
	{
		EstaAutenticado = true;
		Usuario = await repUsuarios.ObterAsync(IdUsuario);
		
	}
	public  Task<bool> EhAutorizadoAsync(Permissao permissao)
	{
		if (!EstaAutenticado)
			return  Task.FromResult(false);
		return AutorizacaoService.EhAutorizadoAsync(IdUsuario, permissao);
	}
	public Task ErroSeNaoAutorizadoAsync(Permissao permissao)
	{
		_ = Usuario ?? throw new NaoAutenticadoException("Usuario não definido");
		if (!EstaAutenticado)
			throw new NaoAutenticadoException(Usuario);
		return AutorizacaoService.ErroSeNaoAutorizadoAsync(Usuario, permissao);
	}


}