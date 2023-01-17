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
		this.autorizacaoService = autorizacaoService;
		this.autenticacaoService = autenticacaoService;
	}
	IAutorizacaoService autorizacaoService { get; set; }
	IAutenticacaoService autenticacaoService { get; set; }
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
		_ = senha ?? throw new ArgumentNullException(senha);
		EstaAutenticado = await autenticacaoService.EhAutenticoAsync(IdUsuario, AutenticacaoService.GerarHahSenha(IdUsuario,senha));
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
		return autorizacaoService.EhAutorizadoAsync(IdUsuario, permissao);
	}
	public Task ErroSeNaoAutorizadoAsync(Permissao permissao)
	{
		_ = Usuario ?? throw new NaoAutenticadoException("Usuario não definido");
		if (!EstaAutenticado)
			throw new NaoAutenticadoException(Usuario);
		return autorizacaoService.ErroSeNaoAutorizadoAsync(Usuario, permissao);
	}


}