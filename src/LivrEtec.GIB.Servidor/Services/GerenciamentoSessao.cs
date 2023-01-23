using LivrEtec.GIB.RPC;
using Microsoft.AspNetCore.Authorization;

namespace LivrEtec.GIB.Servidor.Services;

internal sealed class GerenciamentoSessao : RPC.GerenciamentoSessao.GerenciamentoSessaoBase
{
    private readonly ILogger<GerenciamentoSessao> logger;
    private readonly AuthKeyProvider authKeyProvider;
    private readonly IAutenticacaoService autenticacaoService;
    public GerenciamentoSessao(ILogger<GerenciamentoSessao> logger, IAutenticacaoService autenticacaoService, AuthKeyProvider authKeyProvider)
    {
        this.logger = logger;
        this.autenticacaoService = autenticacaoService;
        this.authKeyProvider = authKeyProvider;
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

}