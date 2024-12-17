using LivrEtec.Repositorios;
using LivrEtec.Servidor.Repositorios;
using Microsoft.AspNetCore.Authorization;
using System.Security.Policy;
using System.Net;


namespace LivrEtec.GIB.Services;
[Route("api/sessao")]
[ApiController]
public sealed class GerenciamentoSessao: ControllerBase
{
    private readonly ILogger<GerenciamentoSessao> logger;
    private readonly AuthKeyProvider authKeyProvider;
    private readonly IAutenticacaoService autenticacaoService;
    private readonly IIdentidadeService identidadeService;
    private readonly IRepUsuarios repUsuarios; 
    public GerenciamentoSessao(ILogger<GerenciamentoSessao> logger, IAutenticacaoService autenticacaoService, AuthKeyProvider authKeyProvider, IIdentidadeService identidadeService, IRepUsuarios repUsuarios)
    {
        this.logger = logger;
        this.autenticacaoService = autenticacaoService;
        this.identidadeService = identidadeService;
        this.authKeyProvider = authKeyProvider;
        this.repUsuarios = repUsuarios;
    }
    public record LoginRequest(int IdUsuario, string HashSenha);
    [AllowAnonymous]
    [HttpPost("login")]
    public  async Task<ActionResult<string>> Login(LoginRequest request)
    {
        if (await autenticacaoService.EhAutentico(request.IdUsuario, request.HashSenha))
            return TokenService.GerarToken(request.IdUsuario, authKeyProvider.authKey);
        return Unauthorized("Usuario não encontrado ou Senha incorreta");
    }

    [HttpGet("autorizado/{id_permissao}")]
    [AllowAnonymous]
    public  async Task<ActionResult<bool>> EhAutorizado(int id_permissao)
    {
        LEM.Permissao? permissao = Permissoes.TodasPermissoes.FirstOrDefault(p => p.Id == id_permissao);
        if (permissao is null)
            return Conflict("Permissão não existe");
        return await identidadeService.EhAutorizado(permissao);
    }

    [HttpGet("usuario")]
    public  async Task<LEM::Usuario> CarregarUsuario()
    {
        await identidadeService.CarregarUsuario();
        return identidadeService.Usuario!;
    }

    [HttpGet("usuario/{login}")]
    public async Task<int?> ObterId(string login)
    {
        return await repUsuarios.ObterId(login);
    }
}