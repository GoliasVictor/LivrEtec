using LivrEtec.Exceptions;
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

namespace LivrEtec.GIB.Cliente.Services;

internal class IdentidadeService : IIdentidadeService
{
    GrpcChannelProvider GrpcChannelProvider;
    public IdentidadeService(GrpcChannelProvider grpcChannelProvider)
    {
        GrpcChannelProvider = grpcChannelProvider;
    }

    public IdentidadeService(){ }

    public string TokenJWT { get; set; }
    public int IdUsuario { get; private set; }
    public Models.Usuario? Usuario { get; private set; }
    public bool EstaAutenticado { get; private set; }

    public Task DefinirUsuario(int idUsuario)
    {
        EstaAutenticado = false;
        IdUsuario = idUsuario;
        return Task.CompletedTask;
    }

    public async Task AutenticarUsuario(string senha)
    {
        _ = senha ?? throw new ArgumentNullException(senha);
        var gerenciamentoSessao = new GerenciamentoSessao.GerenciamentoSessaoClient(GrpcChannelProvider.GetGrpcChannel(null));
        await gerenciamentoSessao.LoginAsync(new LoginRequest {
            IdUsuario = IdUsuario,
            HashSenha = IAutenticacaoService.GerarHahSenha(IdUsuario, senha)
        });
    }
    public Task AutenticarUsuario()
    {
        return AutenticarUsuario(Usuario.Senha);
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