using LivrEtec.Exceptions;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;
using System.Security.Authentication;

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

    public async Task Login(string login, string senha, bool senhaHash)
    {
        _ = senha ?? throw new ArgumentNullException(senha);
        _ = login ?? throw new ArgumentNullException(login);
        var nullableId = await repUsuarios.ObterId(login);
        if (nullableId is null) {
            EstaAutenticado = false;
            Usuario = null;
            return;
        }
        var id = (int)nullableId;
        
        if (!senhaHash)
            senha = IAutenticacaoService.GerarHahSenha((int)nullableId, senha);
        EstaAutenticado = await autenticacaoService.EhAutentico(id, senha);
        if (EstaAutenticado)
            Usuario = new Usuario() { Login = login, Id = id };
    }
    public async Task CarregarUsuario()
    {
        _ = Usuario ?? throw new InvalidOperationException("Usuario indefindo");
        if (!EstaAutenticado)
            throw new NaoAutenticadoException("Usuario não autenticado");
        Usuario = await repUsuarios.Obter(Usuario.Id) 
            ?? throw new InvalidOperationException("Usuario não existe no banco de dados");
    }
    public Task<bool> EhAutorizado(Permissao permissao)
    {
        if (Usuario is null || !EstaAutenticado)
            return Task.FromResult(false);
        return  autorizacaoService.EhAutorizado(Usuario.Id, permissao);
    }
    public Task ErroSeNaoAutorizado(Permissao permissao)
    {
        if (Usuario is null)
            throw new NaoAutenticadoException("Usuario não definido");
        if (!EstaAutenticado)
            throw new NaoAutenticadoException(Usuario);
         return autorizacaoService.ErroSeNaoAutorizado(Usuario, permissao);
    }
  
}