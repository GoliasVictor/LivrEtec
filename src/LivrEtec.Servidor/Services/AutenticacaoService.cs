using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace LivrEtec.Servidor;
public sealed class AutenticacaoService : Service, IAutenticacaoService
{

	public AutenticacaoService(PacaContext bd, ILogger<AutenticacaoService> logger) : base(bd, logger)
	{
	}
	string GerarHahSenha(int IdUsuario, string senha)
	{
		using MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] bytesSenha = System.Text.Encoding.ASCII.GetBytes(senha + IdUsuario.ToString());
        byte[] bytesHash = md5.ComputeHash(bytesSenha);
        return Convert.ToHexString(bytesHash); 

	}

	public bool EhAutentico(int IdUsuario, string senha)
	{		
		_ = senha ?? throw new ArgumentNullException(nameof(senha));
		var hashSenha = GerarHahSenha(IdUsuario, senha);
		var usuario = BD.Usuarios.Find(IdUsuario);
		if(usuario == null)
			throw new ArgumentException("Usuario invalido");
		bool autentico = usuario.Senha.ToUpper() == hashSenha.ToUpper();
		return autentico;
	}
}