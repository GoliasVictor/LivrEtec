namespace LivrEtec;

public interface IAutenticacaoService
{
	bool EhAutentico(int IdUsuario, string senha);
}
