using LivrEtec.Exceptions;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.Services;
public sealed class AutorizacaoService : IAutorizacaoService
{
    private readonly ILogger<AutorizacaoService> logger;
    private readonly IRepUsuarios repUsuarios;
    public AutorizacaoService(IRepUsuarios repUsuarios, ILogger<AutorizacaoService> logger)
    {
        this.repUsuarios = repUsuarios;
        this.logger = logger;
    }
    
    public async Task<bool> EhAutorizado(int idUsuario, Permissao permissao)
    {
        if (permissao == null)
        {
            throw new ArgumentNullException(nameof(permissao));
        }

        if (Permissoes.TodasPermissoes.All(p => p.Id != permissao.Id))
            throw new ArgumentException(nameof(permissao));

        Usuario usuario = await repUsuarios.Obter(idUsuario)
            ?? throw new ArgumentException("Usuario Não Existe");
        return usuario.Cargo.Permissoes.Any((p) => p.Id == permissao.Id);
    }
    public async Task ErroSeNaoAutorizado(int idUsuario, Permissao permissao)
    {
        if (await EhAutorizado(idUsuario, permissao) == false)
            throw new NaoAutorizadoException();
    }
}