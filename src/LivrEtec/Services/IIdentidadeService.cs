namespace LivrEtec;

public interface IIdentidadeService
{
	int IdUsuario { get; }
	Usuario? Usuario { get; }
	bool EstaAutenticado { get; }

	void AutenticarUsuario(string senha);
	void DefinirUsuario(int idUsuario);
	bool EhAutorizado(Permissao permissao);
	void ErroSeNaoAutorizado(Permissao permissao);
}
