using LivrEtec.Models;

namespace LivrEtec;
public static partial class Permissoes
{
    public record PermissoesEmprestimo : CRUD
	{
		public readonly Permissao Fechar;
		public PermissoesEmprestimo(string Nome, int idInicial, Permissao VisualizarLivro, Permissao VisualizarPessoa) : base(Nome,idInicial)
		{
			Visualizar.PermissoesDependete.AddRange(new[]{ VisualizarLivro, VisualizarPessoa}); 
			Fechar = new (idInicial+4,Nome+":"+nameof(Fechar), "", new(){ Criar }); 
			Todas.Add(Fechar);
		}
	}
	
}