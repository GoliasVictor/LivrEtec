using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LivrEtec.GIB.Servidor.Services;
public static class TokenService
{
    public static string GerarToken(int idUsuario, byte[] key)
    {

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]{
                new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(6),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

internal class AuthKeyProvider
{
    public byte[] authKey;

    public AuthKeyProvider(byte[] authKey)
    {
        this.authKey = authKey;
    }
}
