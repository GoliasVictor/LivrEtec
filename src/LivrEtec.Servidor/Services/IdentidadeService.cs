using LivrEtec.Exceptions;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.Services;

public class IdentidadeService : IIdentidadeService
{
    private readonly ILogger<IdentidadeService>? logger;
    private readonly IRepUsuarios repUsuarios;
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

    private IAutorizacaoService autorizacaoService { get; set; }
    private IAutenticacaoService autenticacaoService { get; set; }
    public Usuario? Usuario { get; set; }
    public bool EstaAutenticado { get; set; }

    public async Task AutenticarEDefinirUsuario(string login, string senha)
    {
        _ = senha ?? throw new ArgumentNullException(senha);
        _ = login ?? throw new ArgumentNullException(login);
        var id = await repUsuarios.ObterId(login);
        
        EstaAutenticado = id is not null && await autenticacaoService.EhAutentico((int)id, IAutenticacaoService.GerarHahSenha((int)id, senha));
        if (EstaAutenticado)
        {
            Usuario = await repUsuarios.Obter((int)id);
        }
    }
    public async Task CarregarUsuario()
    {
        _ = Usuario ?? throw new Exception("Usuario indefindo");
        if (EstaAutenticado)
        {
            Usuario = await repUsuarios.Obter(Usuario.Id);
        }
    }
    public Task<bool> EhAutorizado(Permissao permissao)
    {
        _ = Usuario ?? throw new Exception("Usuario indefindo");
        return !EstaAutenticado ? Task.FromResult(false) : autorizacaoService.EhAutorizado(Usuario.Id, permissao);
    }
    public Task ErroSeNaoAutorizado(Permissao permissao)
    {
        _ = Usuario ?? throw new NaoAutenticadoException("Usuario não definido");
        return !EstaAutenticado ? throw new NaoAutenticadoException(Usuario) : autorizacaoService.ErroSeNaoAutorizado(Usuario, permissao);
    }
  
}