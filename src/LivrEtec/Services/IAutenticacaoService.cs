namespace LivrEtec;

public interface IAutenticacaoService
{
	Task<bool> EhAutentico(int IdUsuario, string hash);
}
