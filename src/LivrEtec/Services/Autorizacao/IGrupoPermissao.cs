namespace LivrEtec;

public interface IGrupoPermissao {
	string Nome { get; }
	List<Permissao> Todas { get; }
};
