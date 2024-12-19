using LivrEtec.Repositorios;
using Microsoft.AspNetCore.Authorization;
using LivrEtec.GIB.Services;


namespace LivrEtec.GIB.Controllers;

[Route("auth")]
[ApiController]
public sealed class AuthController: ControllerBase
{
    private readonly ILogger<AuthController> logger;
    private readonly AuthKeyProvider authKeyProvider;
    private readonly IAutenticacaoService autenticacaoService;
    private readonly IIdentidadeService identidadeService;
    private readonly IRepUsuarios repUsuarios; 
    public AuthController(ILogger<AuthController> logger, IAutenticacaoService autenticacaoService, AuthKeyProvider authKeyProvider, IIdentidadeService identidadeService, IRepUsuarios repUsuarios)
    {
        this.logger = logger;
        this.autenticacaoService = autenticacaoService;
        this.identidadeService = identidadeService;
        this.authKeyProvider = authKeyProvider;
        this.repUsuarios = repUsuarios;
    }
    public record RequestLogin(string Login, string HashSenha);
    public record ResponseLogin(string JwtToken);
    [AllowAnonymous]
    [HttpPost("login")]
    public  async Task<ActionResult<ResponseLogin>> Login(RequestLogin request)
    {
        var id = await repUsuarios.ObterId(request.Login);
        if (id is not null && await autenticacaoService.EhAutentico(id.Value, request.HashSenha))
            return new ResponseLogin(TokenService.GerarToken(id.Value, authKeyProvider.authKey));
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
    public  async Task<DTO::Usuario> CarregarUsuario()
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