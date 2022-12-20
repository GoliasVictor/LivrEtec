using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public abstract class Repositorio 
{
	protected AcervoService acervoService;
	protected IDbContextFactory<PacaContext> BDFactory => acervoService.BDFactory;
	protected ILogger? Logger => acervoService.Logger;
	public Repositorio(AcervoService acervoService)
	{
		this.acervoService = acervoService;
	}
}
