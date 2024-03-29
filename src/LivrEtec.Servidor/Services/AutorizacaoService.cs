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

    public async Task<bool> EhAutorizado(Usuario usuario, Permissao permissao)
    {
        return usuario is not null && await EhAutorizado(usuario.Id, permissao);
    }

    public async Task<bool> EhAutorizado(int idUsuario, Permissao permissao)
    {
        if (permissao == null)
        {
            throw new ArgumentNullException(nameof(permissao));
        }

        if (!Permissoes.TodasPermissoes.Contains(permissao))
        {
            throw new ArgumentException(nameof(permissao));
        }

        Usuario usuario = await repUsuarios.Obter(idUsuario)
            ?? throw new ArgumentException("Usuario Não Existe");
        return usuario.Cargo.Permissoes.Any((p) => p.Id == permissao.Id);
    }
    public async Task ErroSeNaoAutorizado(Usuario usuario, Permissao permissao)
    {
        if (await EhAutorizado(usuario, permissao) == false)
        {
            throw new NaoAutorizadoException(usuario, permissao);
        }
    }
}