using Microsoft.EntityFrameworkCore;

namespace LivrEtec.Servidor.BD;

public class PacaContextFactory : IDbContextFactory<PacaContext>
{
    private readonly DbContextOptions<PacaContext> contextOptions;
    public PacaContextFactory(DbContextOptions<PacaContext> contextOptions)
    {
        this.contextOptions = contextOptions;
    }
    public PacaContext CreateDbContext()
    {
        return new PacaContext(contextOptions);
    }
}