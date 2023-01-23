namespace LivrEtec.Models.Autorizacao;

public interface IGrupoPermissao
{
    string Nome { get; }
    List<Permissao> Todas { get; }
};
