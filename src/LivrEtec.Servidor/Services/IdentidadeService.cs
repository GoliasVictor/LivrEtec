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

	public async Task DefinirUsuario(int idUsuario)
	{
		if (false == await repUsuarios.Existe(idUsuario))
			throw new ArgumentException("Usuario não existe");
		EstaAutenticado = false;
		IdUsuario = idUsuario;
		
	}

 	public async Task AutenticarUsuario(string senha)
	{
		_ = senha ?? throw new ArgumentNullException(senha);
		EstaAutenticado = await autenticacaoService.EhAutentico(IdUsuario, AutenticacaoService.GerarHahSenha(IdUsuario,senha));
		if (EstaAutenticado)
			Usuario = await repUsuarios.Obter(IdUsuario);
		
	}
	public async Task AutenticarUsuario()
	{
		EstaAutenticado = true;
		Usuario = await repUsuarios.Obter(IdUsuario);
		
	}
	public  Task<bool> EhAutorizado(Permissao permissao)
	{
		if (!EstaAutenticado)
			return  Task.FromResult(false);
		return autorizacaoService.EhAutorizadoAsync(IdUsuario, permissao);
	}
	public Task ErroSeNaoAutorizado(Permissao permissao)
	{
		_ = Usuario ?? throw new NaoAutenticadoException("Usuario não definido");
		if (!EstaAutenticado)
			throw new NaoAutenticadoException(Usuario);
		return autorizacaoService.ErroSeNaoAutorizadoAsync(Usuario, permissao);
	}


}