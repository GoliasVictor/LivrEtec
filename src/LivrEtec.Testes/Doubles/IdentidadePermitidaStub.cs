#pragma warning disable CS1998 // O método assíncrono não possui operadores 'await' e será executado de forma síncrona

using System.Formats.Asn1;

namespace LivrEtec.Testes.Doubles;

internal class IdentidadePermitidaStub : IIdentidadeService
{
    public IdentidadePermitidaStub()
    {
        Usuario = new Usuario()
        {
            Nome = "Usuario de teste"
        };
    }

    public IdentidadePermitidaStub(Usuario? usuario)
    {
        IdUsuario = usuario.Id;
        Usuario = usuario;
    }

    public int? IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    public bool EstaAutenticado { get; set; } = true;

    public async Task Login(string login, string senha, bool HashSenha) { }

    public async Task CarregarUsuario() { }
    public Task<Usuario?> ObterUsuario()
    {
        return Task.FromResult(Usuario);
    }

    public async Task<bool> EhAutorizado(Permissao permissao)
    {
        return true;
    }

    public async Task ErroSeNaoAutorizado(Permissao permissao) { }
}
