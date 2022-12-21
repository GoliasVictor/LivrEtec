using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public sealed class AcervoService : IAcervoService
{
	internal IDbContextFactory<PacaContext> BDFactory { get; init;}
	internal ILogger? Logger { get; init;}
	public IRepEmprestimo Emprestimos {get;init;}  
	public IRepLivros Livros { get;init;}  
	public IRepPessoas Pessoas {get; init; }  
	public IRepAutores Autores {get;init;}  

	public AcervoService(IDbContextFactory<PacaContext> bdFactory, ILogger<AcervoService>? logger) 
	{
        BDFactory = bdFactory;
		Logger = logger;

        Livros = new RepLivros(this);
        Emprestimos = new RepEmprestimos(this);
        Pessoas = new RepPessoas(this);
        Autores = new RepAutores(this);
    }
 

}