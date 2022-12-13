using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public abstract class Service {
	protected Service(IPacaContext bd, ILogger? logger)
	{
		BD =  bd;
		Logger = logger;
	}

	protected IPacaContext BD { get; init;}
	protected ILogger? Logger { get; init;}
} 