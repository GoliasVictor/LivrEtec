using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public abstract class Repositorio 
{
	protected AcervoService acervoService;
	protected PacaContext BD => acervoService.BD;
	protected ILogger? Logger => acervoService.Logger;
	public Repositorio(AcervoService acervoService)
	{
		this.acervoService = acervoService;
	}
}
