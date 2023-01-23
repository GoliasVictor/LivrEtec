using LivrEtec.Servidor.BD;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.Repositorios;

public class RepPessoas : Repositorio, IRepPessoas
{
    public RepPessoas(PacaContext BD, ILogger<RepPessoas> logger) : base(BD, logger)
    {

    }
    public async Task<Pessoa?> ObterObter(int id)
    {
        return await BD.Pessoas.FindAsync(id);
    }
}
