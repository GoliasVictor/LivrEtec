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

	public async Task DefinirUsuarioAsync(int idUsuario)
	{
		await Task.Run(()=>{
			if (BD.Usuarios.Find(idUsuario) == null)
				throw new ArgumentException("Usuario não existe");
			EstaAutenticado = false;
			IdUsuario = idUsuario;
		});
		
	}

 	public async Task AutenticarUsuarioAsync(string senha)
	{
		EstaAutenticado = await AutenticacaoService.EhAutenticoAsync(IdUsuario, senha);
		if(EstaAutenticado){
			Usuario = await BD.Usuarios.FindAsync(IdUsuario);
			if(Usuario != null){
				BD.Entry(Usuario).Reference((u)=> u.Cargo).Load();
				BD.Entry(Usuario.Cargo).Collection((c)=> c.Permissoes).Load();
			}
		}
	}
	public  Task<bool> EhAutorizadoAsync(Permissao permissao)
	{
		if (!EstaAutenticado)
			return  Task.FromResult(false);
		return AutorizacaoService.EhAutorizadoAsync(IdUsuario, permissao);
	}
	public Task ErroSeNaoAutorizadoAsync(Permissao permissao)
	{
		_ = Usuario ?? throw new NullReferenceException("Usuario não definido");
		if (!EstaAutenticado)
			throw new NaoAutenticadoException(Usuario);
		return AutorizacaoService.ErroSeNaoAutorizadoAsync(Usuario, permissao);
	}


}