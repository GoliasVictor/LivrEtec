using LivrEtec.Servidor.BD;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.Repositorios;

public sealed class RepSenhas : Repositorio, IRepSenhas
{
    public RepSenhas(PacaContext BD, ILogger<RepSenhas> logger) : base(BD, logger)
    {
    }
    
    public async Task Editar(int idUsuario, string hash)
    {
        Senha? senhaAntiga = await BD.Senhas.FindAsync(idUsuario)
            ?? throw new InvalidOperationException($"senha ou usuario {{{idUsuario}}} não existe no banco de dados");
        senhaAntiga.Hash = hash;
        _ = await BD.SaveChangesAsync();
    }

    public async Task<string> Obter(int idUsuario)
    {
        Senha? senha = await BD.Senhas.FindAsync(idUsuario)
            ?? throw new InvalidOperationException($"senha ou usuario {{{idUsuario}}} não existe no banco de dados");
        return senha.Hash;
    }
}