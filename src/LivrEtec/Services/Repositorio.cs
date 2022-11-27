using Microsoft.Extensions.Logging;

namespace LivrEtec;

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
