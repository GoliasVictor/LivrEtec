using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public sealed class AcervoService : IAcervoService
{
	internal IDbContextFactory<PacaContext> BDFactory { get; init;}
	internal ILogger? Logger { get; init;}
	public IRepLivros Livros {get;init;}  
	public IRepAutores Autores {get;init;}  

	public AcervoService(IDbContextFactory<PacaContext> bdFactory, ILogger<AcervoService>? logger) 
	{
        BDFactory = bdFactory;
		Logger = logger;

        Livros = new RepLivros(this);
        Autores = new RepAutores(this);
    }
 

}