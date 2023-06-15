using LivrEtec.GIB.RPC;
using LivrEtec.Repositorios;
using LivrEtec.Servidor.Repositorios;
using Microsoft.AspNetCore.Authorization;
using System.Security.Policy;

namespace LivrEtec.GIB.Servidor.Services;

internal sealed class GerenciamentoSessao : RPC.GerenciamentoSessao.GerenciamentoSessaoBase
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
    public override async Task<Token> Login(LoginRequest request, ServerCallContext context)
    {
        return false == await autenticacaoService.EhAutentico(request.IdUsuario, request.HashSenha)
            ? throw new RpcException(new Status(StatusCode.Unauthenticated, "Usuario não encontrado ou Senha incorreta  "))
            : new Token
            {
                Valor = TokenService.GerarToken(request.IdUsuario, authKeyProvider.authKey)
            };
    }

    [AllowAnonymous]
    public override async Task<RespostaEhAutorizado> EhAutorizado(IdPermissao request, ServerCallContext context)
    {
        LEM.Permissao permissao = Permissoes.TodasPermissoes.FirstOrDefault(p => p.Id == request.Id)
                ?? throw new RpcException(new Status(StatusCode.FailedPrecondition, "Permissão não existe"));
        return new RespostaEhAutorizado
        {
            Autorizado = await identidadeService.EhAutorizado(permissao)
        };
    }

    public override async Task<Usuario> CarregarUsuario(Empty request, ServerCallContext context)
    {
        await identidadeService.CarregarUsuario();
        return identidadeService.Usuario!;
    }

    public override async Task<IdUsuario> ObterId(LoginUsuario request, ServerCallContext context)
    {
        return new IdUsuario()
        {
            Id = await repUsuarios.ObterId(request.Login)
        };
    }
}