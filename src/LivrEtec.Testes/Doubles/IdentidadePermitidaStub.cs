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
        Usuario = usuario;

    }

    public int IdUsuario => Usuario!.Id;

    public Usuario? Usuario { get; set; }

    public bool EstaAutenticado { get; set; } = true;

    public async Task AutenticarEDefinirUsuario(string login, string senha) { }

    public async Task CarregarUsuario() { }

    public async Task<bool> EhAutorizado(Permissao permissao)
    {
        return true;
    }

    public async Task ErroSeNaoAutorizado(Permissao permissao) { }
}
