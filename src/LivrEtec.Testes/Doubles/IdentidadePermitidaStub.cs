﻿#pragma warning disable CS1998 // O método assíncrono não possui operadores 'await' e será executado de forma síncrona

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

    public bool EstaAutenticado => true;

    public async Task AutenticarUsuario(string senha) { }

    public async Task AutenticarUsuario() { }

    public async Task DefinirUsuario(int idUsuario) { }

    public async Task<bool> EhAutorizado(Permissao permissao)
    {
        return true;
    }

    public async Task ErroSeNaoAutorizado(Permissao permissao) { }
}
