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
    public static string GerarHahSenha(int IdUsuario, string senha)
    {
        using var md5 = MD5.Create();
        var bytesSenha = System.Text.Encoding.ASCII.GetBytes(senha + IdUsuario.ToString());
        var bytesHash = md5.ComputeHash(bytesSenha);
        return Convert.ToHexString(bytesHash);
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