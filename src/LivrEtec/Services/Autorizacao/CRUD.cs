namespace LivrEtec;
public static partial class Permissoes
{
    public record CRUD : IGrupoPermissao
	{
		public string Nome {get;init; }
		public readonly Permissao Criar = null!;
		public readonly Permissao Visualizar = null!;
		public readonly Permissao Editar = null!;
		public readonly Permissao Excluir = null!;
		public List<Permissao> Todas  { get; init; } = new List<Permissao>();
		public CRUD(string nome,
			  int idInicial, 
			  string descricaoCriar = "",
			  string descricaoVisualizar = "",
			  string descricaoEditar = "",
			  string descricaoExcluir = ""
		)
		{
			Nome = nome;
			Visualizar 	= new(idInicial    , Nome+":"+nameof(Visualizar), descricaoVisualizar );
			Criar 		= new(idInicial + 1, Nome+":"+nameof(Criar)     , descricaoCriar , new(){ Visualizar });
			Editar 		= new(idInicial + 2, Nome+":"+nameof(Editar)	   , descricaoEditar    , new(){ Visualizar });
			Excluir 	= new(idInicial + 3, Nome+":"+nameof(Excluir)   , descricaoExcluir   , new(){ Editar });
			Todas.AddRange(new []{Criar, Visualizar, Editar, Excluir});
		}
	}
	
}