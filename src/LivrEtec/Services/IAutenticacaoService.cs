namespace LivrEtec;

public interface IAutenticacaoService
{
	Task<bool> EhAutenticoAsync(int IdUsuario, string senha);
}
