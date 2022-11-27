namespace LivrEtec;

public interface IGrupoPermissao {
	string Nome { get; }
	List<Permissao> Todas { get; }
};
public static class Permissoes
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


	public record PermissoesEmprestimo : CRUD
	{
		Permissao Fechar;
		public PermissoesEmprestimo(string Nome, int idInicial, Permissao VisualizarLivro, Permissao VisualizarPessoa) : base(Nome,idInicial)
		{
			Visualizar.PermissoesDependete.AddRange(new[]{ VisualizarLivro, VisualizarPessoa}); 
			Fechar = new (idInicial+4,Nome+":"+nameof(Fechar), "", new(){ Criar }); 
			Todas.Add(Fechar);
		}
	}

	public static readonly CRUD Livro ;
	public static readonly CRUD Tag ;
	public static readonly CRUD Autor ;
	public static readonly CRUD Cargo ;
	public static readonly CRUD Pessoa ;
	public static readonly CRUD Usuario ;
	public static readonly PermissoesEmprestimo Emprestimo;
	public static IGrupoPermissao[] TodosGrupos 
		=> new[]{ Livro, Tag, Autor, Usuario, Cargo, Pessoa, Emprestimo } ;
	public static Permissao[] TodasPermissoes
		=> TodosGrupos.SelectMany(grupo => grupo.Todas).ToArray();

	static Permissoes()
	{
		int UltimoID = 1;
		Livro 	   = new(nameof(Livro)		, UltimoID);
		Tag 	   = new(nameof(Tag)		, UltimoID=UltimoID + 4);
		Autor 	   = new(nameof(Autor)		, UltimoID=UltimoID + 4);
		Usuario    = new(nameof(Usuario)	, UltimoID=UltimoID + 4);
		Cargo 	   = new(nameof(Cargo)		, UltimoID=UltimoID + 4);
		Pessoa 	   = new(nameof(Pessoa)		, UltimoID=UltimoID + 4);
		Emprestimo = new(nameof(Emprestimo)	, UltimoID=UltimoID + 4, Livro.Visualizar, Pessoa.Visualizar);
	}
	
}