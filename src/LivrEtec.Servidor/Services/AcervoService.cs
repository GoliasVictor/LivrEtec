using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public sealed class AcervoService : IAcervoService
{
	internal PacaContext BD { get; init;}
	internal ILogger? Logger { get; init;}
	public IRepLivros Livros {get;init;}  
	public IRepAutores Autores {get;init;}  

	public AcervoService(PacaContext bd, ILogger<AcervoService>? logger) 
	{
        BD = bd;
		Logger = logger;

        Livros = new RepLivros(this);
        Autores = new RepAutores(this);
    }
 

}