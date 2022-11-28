using Microsoft.Extensions.Logging;

namespace LivrEtec;

public abstract class Service {
	protected Service(PacaContext bd, ILogger? logger)
	{
		BD =  bd;
		Logger = logger;
	}

	protected PacaContext BD { get; init;}
	protected ILogger? Logger { get; init;}
} 