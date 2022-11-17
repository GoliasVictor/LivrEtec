using Microsoft.Extensions.Logging;

namespace LivrEtec;

public  abstract class Repositorio
{
	protected IAcervoService acervoService;
	protected PacaContext BD => acervoService.BD;
	protected ILogger? Logger => acervoService.Logger;
	public Repositorio(IAcervoService acervoService)
	{
		this.acervoService = acervoService;
	}
}
