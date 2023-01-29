namespace LivrEtec.Services;

/// <summary>.
/// Serviço responsavel por verificar atutenticidade do usuario 
/// </summary>
public interface IAutenticacaoService
{
	/// <summary>
	/// Verifica se o usuario de identificado por <paramref name="idUsuario"/>, com a o hash da senha <paramref name="hash"/> é autentico
	/// </summary>
	/// <param name="idUsuario">Id do usuario.</param>
	/// <param name="hash">O hash da senha do usuario </param>
	/// <returns>Boleano indicando se as credenciais estão corretas.</returns>
	Task<bool> EhAutentico(int idUsuario, string hash);
}
