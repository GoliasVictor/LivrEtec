﻿using LivrEtec.Exceptions;
using LivrEtec.GIB.RPC;
using LivrEtec.Models;
using LivrEtec.Repositorios;
using LivrEtec.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Provider;

namespace LivrEtec.GIB.Cliente.Services;

internal class IdentidadeService : IIdentidadeService
{
    readonly GrpcChannelProvider GrpcChannelProvider;
    public IdentidadeService(GrpcChannelProvider grpcChannelProvider)
    {
        GrpcChannelProvider = grpcChannelProvider;
    }

    public IdentidadeService() { }

    public Models.Usuario? Usuario { get; set; }
    public bool EstaAutenticado { get; set; }

    
    public async Task Login(string login, string senha, bool senhaHash)
    {
        _ = senha ?? throw new ArgumentNullException(senha);
        try
        {
            var gerenciamentoSessao = new GerenciamentoSessao.GerenciamentoSessaoClient(GrpcChannelProvider.GetGrpcChannel());
            var id = (int)(await gerenciamentoSessao.ObterIdAsync(new LoginUsuario() { Login = login })).Id;
            if (!senhaHash)
                senha = IAutenticacaoService.GerarHahSenha(id, senha);

            Token token = await gerenciamentoSessao.LoginAsync(new LoginRequest
            {
                IdUsuario = id,
                HashSenha = senha
            });
            GrpcChannelProvider.DefinirToken(token.Valor);

            EstaAutenticado = true;
        }
        catch
        {
            EstaAutenticado = false;
        }
    }

    public async Task CarregarUsuario()
    {
        var gerenciamentoSessao = new GerenciamentoSessao.GerenciamentoSessaoClient(GrpcChannelProvider.GetGrpcChannel());
        Usuario = await gerenciamentoSessao.CarregarUsuarioAsync(new Empty());
    }
    public Task<bool> EhAutorizado(Models.Permissao permissao)
    {
        throw new NotImplementedException();
    }
    public Task ErroSeNaoAutorizado(Models.Permissao permissao)
    {
        throw new NotImplementedException();
    }
}