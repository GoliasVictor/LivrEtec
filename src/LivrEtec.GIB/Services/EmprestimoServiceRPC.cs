using Google.Protobuf.WellKnownTypes;
using LivrEtec.Models;
using Microsoft.Extensions.Logging;
using static LivrEtec.GIB.RPC.Emprestimo.Types;

namespace LivrEtec.GIB.Services;

public sealed class EmprestimoServiceRPC : IEmprestimoService
{
    private readonly ILogger<EmprestimoServiceRPC> logger;
    private readonly RPC::Emprestimos.EmprestimosClient clientRPC;
    public EmprestimoServiceRPC(ILogger<EmprestimoServiceRPC> logger, RPC::Emprestimos.EmprestimosClient clientRPC)
    {
        this.clientRPC = clientRPC;
        this.logger = logger;
    }

    public async Task<int> Abrir(int idPessoa, int idlivro)
    {
        try
        {
            RPC::IdEmprestimo idEmprestimo = await clientRPC.AbrirAsync(new AbrirRequest()
            {
                IdLivro = idlivro,
                IdPessoa = idPessoa
            });
            return idEmprestimo.Id;
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }

    public Task<IEnumerable<Emprestimo>> Buscar(ParamBuscaEmprestimo parametros)
    {
        throw new NotImplementedException();
    }

    public async Task Devolver(int idEmprestimo, bool? AtrasoJustificado = null, string? ExplicacaoAtraso = null)
    {
        try
        {
            var request = new DevolverRequest() { IdEmprestimo = idEmprestimo };
            if (AtrasoJustificado is not null)
            {
                request.AtrasoJustificado = AtrasoJustificado.Value;
            }

            if (ExplicacaoAtraso is not null)
            {
                request.ExplicacaoAtraso = ExplicacaoAtraso;
            }

            _ = await clientRPC.DevolverAsync(request);
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }

    public async Task Prorrogar(int idEmprestimo, DateTime novaData)
    {

        try
        {
            _ = await clientRPC.ProrrogarAsync(new ProrrogarRequest()
            {
                IdEmprestimo = idEmprestimo,
                NovaData = Timestamp.FromDateTime(novaData.ToUniversalTime())
            });
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }

    public async Task RegistrarPerda(int idEmprestimo)
    {
        try
        {
            _ = await clientRPC.RegistrarPerdaAsync(new RPC::IdEmprestimo()
            {
                Id = idEmprestimo,
            });
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }
    public async Task Excluir(int idEmprestimo)
    {
        try
        {

            _ = await clientRPC.ExcluirAsync(new RPC::IdEmprestimo()
            {
                Id = idEmprestimo,
            });
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }
}