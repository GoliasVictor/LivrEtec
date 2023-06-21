using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace LivrEtec.Servidor.Services;
public sealed class AutenticacaoService : IAutenticacaoService
{
    private readonly ILogger<AutenticacaoService> logger;
    private readonly IRepSenhas repSenhas;
    public AutenticacaoService(IRepSenhas repSenhas, ILogger<AutenticacaoService> logger)
    {
        this.logger = logger;
        this.repSenhas = repSenhas;
    }

    public async Task<bool> EhAutentico(int IdUsuario, string hashSenha)
    {
        _ = hashSenha ?? throw new ArgumentNullException(nameof(hashSenha));
        string senha = await repSenhas.Obter(IdUsuario);
        
        var autentico = senha.ToUpper() == hashSenha.ToUpper();
        return autentico;
    }
}