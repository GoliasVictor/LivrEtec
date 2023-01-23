using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

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
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

class AuthKeyProvider
{
    public byte[] authKey;

    public AuthKeyProvider(byte[] authKey)
    {
        this.authKey = authKey;
    }
}
