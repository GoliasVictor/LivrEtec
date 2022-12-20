using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public class PacaContextFactory : IDbContextFactory<PacaContext>
{
	Action<DbContextOptionsBuilder> _configurarAction;
	IConfiguracao _config;
	ILoggerFactory _loggerFactory;
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