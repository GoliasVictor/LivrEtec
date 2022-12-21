using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public class PacaContextFactory : IDbContextFactory<PacaContext>
{
	readonly Action<DbContextOptionsBuilder> _configurarAction;
	readonly IConfiguracao _config;
	readonly ILoggerFactory _loggerFactory;
	public PacaContextFactory(IConfiguracao config, Action<DbContextOptionsBuilder> configurarAction, ILoggerFactory loggerFactory){
		_configurarAction = configurarAction;
		_config = config;
		_loggerFactory = loggerFactory;
	}
	public PacaContext CreateDbContext()
	{
		return new PacaContext(_config, _loggerFactory, _configurarAction);
	}
}