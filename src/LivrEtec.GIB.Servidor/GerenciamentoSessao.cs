using System.Security.Claims;
using Grpc.Core;
using LivrEtec.GIB.RPC;
using Microsoft.AspNetCore.Authorization;

namespace LivrEtec.GIB.Servidor
{
	sealed class GerenciamentoSessao : RPC.GerenciamentoSessao.GerenciamentoSessaoBase
	{
		readonly ILogger<GerenciamentoSessao> logger;
		readonly AuthKeyProvider authKeyProvider;
		readonly IAutenticacaoService autenticacaoService;
		public GerenciamentoSessao(ILogger<GerenciamentoSessao> logger, IAutenticacaoService autenticacaoService, AuthKeyProvider authKeyProvider)
		{
			this.logger = logger;
			this.autenticacaoService = autenticacaoService;
			this.authKeyProvider = authKeyProvider;
		}
		[AllowAnonymous]
		public override async Task<Token> Login(LoginRequest request, ServerCallContext context)
		{
			if( false == await autenticacaoService.EhAutentico(request.IdUsuario, request.HashSenha))
				throw new RpcException(new Status(StatusCode.Unauthenticated,"Usuario não encontrado ou Senha incorreta  "));
			return new Token {
				Valor = TokenService.GerarToken(request.IdUsuario, authKeyProvider.authKey)
			};
		}

	}
}