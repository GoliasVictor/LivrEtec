using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace LivrEtec;


public interface IAcervoService
{
	PacaContext BD { get;init; }
	ILogger? Logger { get;init; }
	ILivroService Livros { get; init; }

}

public class AcervoService : IAcervoService
{
	public PacaContext BD { get;  init;}
	public ILogger? Logger { get; init;}
	public ILivroService Livros {get;init;}  
	public AutorService Autores {get;init;}  

	public AcervoService(PacaContext bd, ILogger<AcervoService>? logger) 
	{
        BD = bd;
		Logger = logger;

        Livros = new LivroService(this);
        Autores = new AutorService(this);
    }
 

}