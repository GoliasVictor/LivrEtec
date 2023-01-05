using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace LivrEtec.Servidor;
public sealed class AutenticacaoService : IAutenticacaoService
{
	readonly ILogger<AutenticacaoService> logger;
	readonly IRepUsuarios repUsuarios;
	public AutenticacaoService(IRepUsuarios repUsuarios, ILogger<AutenticacaoService> logger) 
	{
		this.logger = logger;
		this.repUsuarios =  repUsuarios;
	}
	string GerarHahSenha(int IdUsuario, string senha)
	{
		using MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] bytesSenha = System.Text.Encoding.ASCII.GetBytes(senha + IdUsuario.ToString());
        byte[] bytesHash = md5.ComputeHash(bytesSenha);
        return Convert.ToHexString(bytesHash); 
	}
	public async Task<bool> EhAutenticoAsync(int IdUsuario, string senha)
	{
		_ = senha ?? throw new ArgumentNullException(nameof(senha));
		var hashSenha = GerarHahSenha(IdUsuario, senha);
		var usuario = await repUsuarios.ObterAsync(IdUsuario);
		if(usuario == null)
			throw new ArgumentException("Usuario invalido");
		bool autentico = usuario.Senha.ToUpper() == hashSenha.ToUpper();
		return autentico;
	}
}