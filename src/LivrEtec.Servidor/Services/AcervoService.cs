using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public sealed class AcervoService : IAcervoService
{
	internal PacaContext BD { get; init;}
	internal ILogger? Logger { get; init;}
	public IRepEmprestimo Emprestimos {get;init;}  
	public IRepLivros Livros { get;init;}  
	public IRepPessoas Pessoas {get; init; }  
	public IRepAutores Autores {get;init;}
	public IRepUsuarios Usuarios {get;init;}
	private IRelogio _relogio;

	public AcervoService(PacaContext bd, ILogger<AcervoService>? logger, IRelogio relogio ) 
	{
		_relogio = relogio ?? new RelogioSistema(); 
        BD = bd;
		Logger = logger;

        Livros = new RepLivros(this);
        Emprestimos = new RepEmprestimos(this,_relogio);
        Pessoas = new RepPessoas(this);
        Autores = new RepAutores(this);
        Usuarios = new RepUsuarios(this);
    }
 

}