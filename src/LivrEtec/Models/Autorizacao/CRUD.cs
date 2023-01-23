using LivrEtec.Models.Autorizacao;

namespace LivrEtec;
public static partial class Permissoes
{
    public record CRUD : IGrupoPermissao
    {
        public string Nome { get; init; }
        public readonly Permissao Criar;
        public readonly Permissao Visualizar;
        public readonly Permissao Editar;
        public readonly Permissao Excluir;
        public List<Permissao> Todas { get; init; } = new List<Permissao>();
        public CRUD(string nome,
              int idInicial,
              string descricaoCriar = "",
              string descricaoVisualizar = "",
              string descricaoEditar = "",
              string descricaoExcluir = ""
        )
        {
            Nome = nome;
            Visualizar = new(idInicial, Nome + ":" + nameof(Visualizar), descricaoVisualizar);
            Criar = new(idInicial + 1, Nome + ":" + nameof(Criar), descricaoCriar, new() { Visualizar });
            Editar = new(idInicial + 2, Nome + ":" + nameof(Editar), descricaoEditar, new() { Visualizar });
            Excluir = new(idInicial + 3, Nome + ":" + nameof(Excluir), descricaoExcluir, new() { Editar });
            Todas.AddRange(new[] { Criar, Visualizar, Editar, Excluir });
        }
    }

}