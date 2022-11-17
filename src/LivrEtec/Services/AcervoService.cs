using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace LivrEtec;


public interface IAcervoService
{
	PacaContext BD { get;init; }
	ILogger? Logger { get;init; }

}

public class AcervoService : IAcervoService
{
	public PacaContext BD { get;  init;}
	public ILogger? Logger { get; init;}
	public LivroService Livros {get;init;}  
	public AutorService Autores {get;init;}  
	public AcervoService(){
		Livros =  new LivroService(this);
		Autores =  new AutorService(this);
		
		BD = null!;
	}
	public AcervoService(PacaContext bd, ILogger? logger) : this()
	{
		BD = bd;
		Logger = logger;
	}
 

}