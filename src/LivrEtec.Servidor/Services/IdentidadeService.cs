using System.Data.Entity;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public class IdentidadeService : Service, IIdentidadeService
{
	public IdentidadeService(
		PacaContext bd,
	 	ILogger<IdentidadeService>? logger,
		IAutorizacaoService autorizacaoService,
		IAutenticacaoService autenticacaoService
	) : base(bd, logger)
	{
		AutorizacaoService = autorizacaoService;
		AutenticacaoService = autenticacaoService;
	}
	IAutorizacaoService AutorizacaoService { get; set; }
	IAutenticacaoService AutenticacaoService { get; set; }
	public int IdUsuario { get; private set; }
	public Usuario? Usuario { get; private set; }
	public bool EstaAutenticado { get; private set; }

	public void DefinirUsuario(int idUsuario)
	{
		if (!BD.Usuarios.Any(u=> u.Id == idUsuario))
			throw new ArgumentException("Usuario não existe");
		EstaAutenticado = false;
		IdUsuario = idUsuario;
	}
	public void AutenticarUsuario(string senha)
	{
		EstaAutenticado = AutenticacaoService.EhAutentico(IdUsuario, senha);
		if(EstaAutenticado)
			Usuario = BD.Usuarios.Find(IdUsuario);
	}
	public bool EhAutorizado(Permissao permissao)
	{
		if (!EstaAutenticado)
			return false;
		return AutorizacaoService.EhAutorizado(IdUsuario, permissao);
	}
	public void ErroSeNaoAutorizado(Permissao permissao)
	{
		_ = Usuario ?? throw new NullReferenceException("Usuario não definido");
		if (!EstaAutenticado)
			throw new NaoAutenticadoException(Usuario);
		AutorizacaoService.ErroSeNaoAutorizado(Usuario, permissao);
	}
	public async Task DefinirUsuarioAsync(int idUsuario)
	{
		if (await BD.Usuarios.AnyAsync(u=> u.Id == idUsuario) == false)
			throw new ArgumentException("Usuario não existe");
		EstaAutenticado = false;
		IdUsuario = idUsuario;
	}
	public async Task AutenticarUsuarioAsync(string senha)
	{
		EstaAutenticado = AutenticacaoService.EhAutentico(IdUsuario, senha);
		if(EstaAutenticado)
			Usuario = await BD.Usuarios.FindAsync(IdUsuario);
	}
	public async Task<bool> EhAutorizadoAsync(Permissao permissao)
	{
		if (!EstaAutenticado)
			return false;
		return await AutorizacaoService.EhAutorizadoAsync(IdUsuario, permissao);
	}
	public async Task ErroSeNaoAutorizadoAsync(Permissao permissao)
	{
		_ = Usuario ?? throw new NullReferenceException("Usuario não definido");
		if (!EstaAutenticado)
			throw new NaoAutenticadoException(Usuario);
		await AutorizacaoService.ErroSeNaoAutorizadoAsync(Usuario, permissao);
	}

}