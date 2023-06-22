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

    private Usuario? usuario = null;
    public async Task<Usuario?> ObterUsuario()
    {
        if (IdUsuario is null || !EstaAutenticado)
            return null;
        if (usuario is null)
            await CarregarUsuario();
        return usuario;
    }

    public int? IdUsuario { get; set; }
    public bool EstaAutenticado { get; set; }

    public async Task Login(string login, string senha, bool senhaHash)
    {
        _ = senha ?? throw new ArgumentNullException(senha);
        _ = login ?? throw new ArgumentNullException(login);
        var nullableId = await repUsuarios.ObterId(login);
        if (nullableId is null) {
            EstaAutenticado = false;
            IdUsuario = null;
            return;
        }
        var id = (int)nullableId;
        
        if (!senhaHash)
            senha = IAutenticacaoService.GerarHahSenha((int)nullableId, senha);
        EstaAutenticado = await autenticacaoService.EhAutentico(id, senha);
        if (EstaAutenticado)
            IdUsuario = id;
    }
    public async Task CarregarUsuario()
    {
        _ = IdUsuario ?? throw new InvalidOperationException("Usuario indefindo");
        if (!EstaAutenticado)
            throw new NaoAutenticadoException("Usuario não autenticado");
        usuario = await repUsuarios.Obter((int)IdUsuario) 
            ?? throw new InvalidOperationException("Usuario não existe no banco de dados");
    }
    public Task<bool> EhAutorizado(Permissao permissao)
    {
        if (IdUsuario is null || !EstaAutenticado)
            return Task.FromResult(false);
        return  autorizacaoService.EhAutorizado((int)IdUsuario, permissao);
    }
    public Task ErroSeNaoAutorizado(Permissao permissao)
    {
        if (IdUsuario is null)
            throw new NaoAutenticadoException("Usuario não definido");
        if (!EstaAutenticado)
            throw new NaoAutenticadoException();
        return autorizacaoService.ErroSeNaoAutorizado((int)IdUsuario, permissao);
    }
  
}