using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace LivrEtec.Servidor.Services;
public sealed class AutenticacaoService : IAutenticacaoService
{
    private readonly ILogger<AutenticacaoService> logger;
    private readonly IRepUsuarios repUsuarios;
    public AutenticacaoService(IRepUsuarios repUsuarios, ILogger<AutenticacaoService> logger)
    {
        this.logger = logger;
        this.repUsuarios = repUsuarios;
    }

    public async Task<bool> EhAutentico(int IdUsuario, string hashSenha)
    {
        _ = hashSenha ?? throw new ArgumentNullException(nameof(hashSenha));
        Usuario? usuario = await repUsuarios.Obter(IdUsuario);
        if (usuario == null)
        {
            throw new ArgumentException("Usuario invalido");
        }

        var autentico = usuario.Senha.ToUpper() == hashSenha.ToUpper();
        return autentico;
    }
}