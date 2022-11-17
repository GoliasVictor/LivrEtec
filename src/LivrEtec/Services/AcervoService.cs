using System.Linq.Expressions;

namespace LivrEtec;

using Pred = Func<Livro, bool>;
using ExPred = Expression<Func<Livro, bool>>;

public class AcervoService
{
	public PacaContext BD;
	public AcervoService(PacaContext bd)
	{
		BD = bd;
	}

	public IQueryable<Livro> BuscarLivro(string textoBusca, IEnumerable<Tag> tags = default)
	{

		IQueryable<Livro> livros = BD.Livros;
		if (!string.IsNullOrEmpty(textoBusca))
		{
			var livPorNome = from livro in BD.Livros
							 where livro.Nome.Contains(textoBusca)
							 select livro;
			var livPorAutor = from autor in BD.Autores
							  from livro in autor.Livros
							  where livro.Nome.Contains(textoBusca)
							  select livro;
			livros = livPorNome.Union(livPorAutor);
		}
		if (tags?.DefaultIfEmpty() != null)
		{
			ParameterExpression param = Expression.Parameter(typeof(Livro), "livro");
			ExPred? predicate = null;
			foreach(var tag in tags){
				var invoke =  Expression.Invoke( (ExPred)((livro) => livro.Tags.Any(tag2 => tag == tag2)),param);
				if(predicate == null)
					predicate =Expression.Lambda<Pred>(invoke, param) ;
				else
				predicate  =  Expression.Lambda<Pred>(Expression.And(predicate.Body, invoke), param);
			}
			if(predicate != null)
				livros = livros.Where(predicate);
		}


		return livros;
	}
}