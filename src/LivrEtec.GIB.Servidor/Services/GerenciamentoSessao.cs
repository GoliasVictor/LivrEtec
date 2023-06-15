using LivrEtec.GIB.RPC;
using Microsoft.AspNetCore.Authorization;

namespace LivrEtec.GIB.Servidor.Services;

internal sealed class GerenciamentoSessao : RPC.GerenciamentoSessao.GerenciamentoSessaoBase
{
    private readonly ILogger<GerenciamentoSessao> logger;
    private readonly AuthKeyProvider authKeyProvider;
    private readonly IAutenticacaoService autenticacaoService;
    private readonly IIdentidadeService identidadeService;
    public GerenciamentoSessao(ILogger<GerenciamentoSessao> logger, IAutenticacaoService autenticacaoService, AuthKeyProvider authKeyProvider, IIdentidadeService identidadeService)
    {
        this.logger = logger;
        this.autenticacaoService = autenticacaoService;
        this.identidadeService = identidadeService;
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

}