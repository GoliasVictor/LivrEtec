using LivrEtec.Exceptions;
using LivrEtec.GIB.RPC;
using Grpc.Core;
using Grpc.Net.Client;
using static LivrEtec.GIB.RPC.GerenciamentoSessao;
using Permissao = LivrEtec.Models.Permissao;
using Usuario = LivrEtec.Models.Usuario;

namespace LivrEtec.GIB.Cliente.Services;

internal class IdentidadeService : IIdentidadeService
{
    readonly GrpcChannelProvider GrpcChannelProvider;
    GrpcChannel GrpcChannel;
    GerenciamentoSessaoClient GerenciamentoSessao;
    public int? IdUsuario { get; set; }

    public IdentidadeService(GrpcChannelProvider grpcChannelProvider)
    {
        GrpcChannelProvider = grpcChannelProvider;
        AtualizarGrpChannel();
    }

    void AtualizarGrpChannel()
    {
        GrpcChannel?.Dispose();
        GrpcChannel = GrpcChannelProvider.GetGrpcChannel();
        GerenciamentoSessao = new GerenciamentoSessaoClient(GrpcChannel);
    }
    public IdentidadeService() { }

    public bool EstaAutenticado { get; set; }

    
    public async Task Login(string login, string senha, bool senhaHash)
    {
        _ = senha ?? throw new ArgumentNullException(nameof(senha));
        try
        {
            var idOptional = await GerenciamentoSessao.ObterIdAsync(new LoginUsuario() { Login = login });
            var id = idOptional.Id  ?? throw new NaoAutenticadoException();
            if (!senhaHash)
                senha = IAutenticacaoService.GerarHahSenha(id, senha);

            Token token = await GerenciamentoSessao.LoginAsync(new LoginRequest
            {
                IdUsuario = id,
                HashSenha = senha
            });
            
            GrpcChannelProvider.DefinirToken(token.Valor);
            AtualizarGrpChannel();
            
            EstaAutenticado = true;
        }
        catch (Exception ex) when (
            ex is NaoAutenticadoException 
                or RpcException { StatusCode: StatusCode.Unauthenticated })
        {
            EstaAutenticado = false;
        }
    }

    public async Task CarregarUsuario()
    {
        usuario = await GerenciamentoSessao.CarregarUsuarioAsync(new Empty());
    }
    private Usuario? usuario = null;
    public async Task<Usuario?> ObterUsuario()
    {
        if (IdUsuario is null || !EstaAutenticado)
            return null;
        if (usuario is null)
            await CarregarUsuario();
        return usuario;
    }
    public async Task<bool> EhAutorizado(Permissao permissao)
    {
        var response = await GerenciamentoSessao.EhAutorizadoAsync(new Id(permissao.Id));
        return response.Autorizado;
    }
    public async Task ErroSeNaoAutorizado(Permissao permissao)
    {
        if (EstaAutenticado)
            throw new NaoAutorizadoException();
        if (await EhAutorizado(permissao))
            throw new NaoAutorizadoException();
    }
}