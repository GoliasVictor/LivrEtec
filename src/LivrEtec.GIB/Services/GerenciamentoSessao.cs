using LivrEtec.GIB.RPC;
using LivrEtec.Repositorios;
using LivrEtec.Servidor.Repositorios;
using Microsoft.AspNetCore.Authorization;
using System.Security.Policy;

namespace LivrEtec.GIB.Services;
[Route("api/sessao")]
[ApiController]
public sealed class GerenciamentoSessao
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
    [AllowAnonymous]
    [HttpPost("login")]
    public  async Task<Token> Login(LoginRequest request)
    {
        return false == await autenticacaoService.EhAutentico(request.IdUsuario, request.HashSenha)
            ? throw new RpcException(new Status(StatusCode.Unauthenticated, "Usuario não encontrado ou Senha incorreta  "))
            : new Token
            {
                Valor = TokenService.GerarToken(request.IdUsuario, authKeyProvider.authKey)
            };
    }

    [HttpGet("autorizado")]
    [AllowAnonymous]
    public  async Task<RespostaEhAutorizado> EhAutorizado(IdPermissao request)
    {
        LEM.Permissao permissao = Permissoes.TodasPermissoes.FirstOrDefault(p => p.Id == request.Id)
                ?? throw new RpcException(new Status(StatusCode.FailedPrecondition, "Permissão não existe"));
        return new RespostaEhAutorizado
        {
            Autorizado = await identidadeService.EhAutorizado(permissao)
        };
    }

    [HttpGet("usuario")]
    public  async Task<Usuario> CarregarUsuario(Empty request)
    {
        await identidadeService.CarregarUsuario();
        return identidadeService.Usuario!;
    }

    [HttpGet("id")]
    public  async Task<IdUsuario> ObterId(LoginUsuario request)
    {
        return new IdUsuario()
        {
            Id = await repUsuarios.ObterId(request.Login)
        };
    }
}