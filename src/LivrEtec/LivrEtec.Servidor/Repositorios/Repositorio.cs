using LivrEtec.Servidor.BD;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.Repositorios;

public abstract class Repositorio
{
    protected PacaContext BD;
    protected ILogger? logger;
    public Repositorio(PacaContext BD, ILogger logger)
    {
        this.BD = BD;
        this.logger = logger;
    }
}
