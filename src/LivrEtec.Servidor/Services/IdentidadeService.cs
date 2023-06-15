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
    public int IdUsuario { get; private set; }
    public Usuario? Usuario { get; private set; }
    public bool EstaAutenticado { get; private set; }

    public async Task DefinirUsuario(int idUsuario)
    {
        if (false == await repUsuarios.Existe(idUsuario))
        {
            throw new ArgumentException("Usuario não existe");
        }

        EstaAutenticado = false;
        IdUsuario = idUsuario;

    }

    public async Task AutenticarUsuario(string senha)
    {
        _ = senha ?? throw new ArgumentNullException(senha);
        EstaAutenticado = await autenticacaoService.EhAutentico(IdUsuario, IAutenticacaoService.GerarHahSenha(IdUsuario, senha));
        if (EstaAutenticado)
        {
            Usuario = await repUsuarios.Obter(IdUsuario);
        }
    }
    public async Task AutenticarUsuario()
    {
        EstaAutenticado = true;
        Usuario = await repUsuarios.Obter(IdUsuario);

    }
    public Task<bool> EhAutorizado(Permissao permissao)
    {
        return !EstaAutenticado ? Task.FromResult(false) : autorizacaoService.EhAutorizado(IdUsuario, permissao);
    }
    public Task ErroSeNaoAutorizado(Permissao permissao)
    {
        _ = Usuario ?? throw new NaoAutenticadoException("Usuario não definido");
        return !EstaAutenticado ? throw new NaoAutenticadoException(Usuario) : autorizacaoService.ErroSeNaoAutorizado(Usuario, permissao);
    }


}