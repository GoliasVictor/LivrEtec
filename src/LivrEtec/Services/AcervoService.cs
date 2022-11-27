using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace LivrEtec;

public sealed class AcervoService : IAcervoService
{
	internal PacaContext BD { get; init;}
	internal ILogger? Logger { get; init;}
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