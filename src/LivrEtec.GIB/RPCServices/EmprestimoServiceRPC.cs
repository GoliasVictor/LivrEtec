using LivrEtec.Models;
using Microsoft.Extensions.Logging;
using static LivrEtec.GIB.Services.EmprestimoServiceRPC;

namespace LivrEtec.GIB.Services.Cliente;

public sealed class EmprestimoServiceRPC : IEmprestimoService
{
    private readonly ILogger<EmprestimoServiceRPC> logger;
    private readonly HttpClient client;
    public EmprestimoServiceRPC(ILogger<EmprestimoServiceRPC> logger, HttpClient client)
    {
        this.client = client;
        this.logger = logger;
    }
    public async Task<int> Abrir(int idPessoa, int idlivro)
    {
        try
        {
            var response = await client.PostAsJsonAsync(
                "/api/emprestimos", 
                new AbrirRequest(idPessoa, idlivro)
            );
            response.EnsureSuccessStatusCode();
            return int.Parse(await response.Content.ReadAsStringAsync());
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
            var request = new DevolverRequest(idEmprestimo, AtrasoJustificado, ExplicacaoAtraso);
            await client.PatchAsJsonAsync("api/emprestimos/devolver", request);
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
            var request = new ProrrogarRequest(idEmprestimo, novaData); 
            _ = await client.PatchAsJsonAsync("api/emprestimos/prorrogar",request);
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
            await client.PatchAsJsonAsync(
                "api/emprestimos/perda",
                new PerdaRequest(idEmprestimo)
            );
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
            await client.DeleteAsync($"api/emprestimos/{idEmprestimo}");
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }
}