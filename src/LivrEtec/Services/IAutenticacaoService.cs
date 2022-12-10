namespace LivrEtec;

public interface IAutenticacaoService
{
	bool EhAutentico(int IdUsuario, string senha);

	Task<bool> EhAutenticoAsync(int IdUsuario, string senha);
}
