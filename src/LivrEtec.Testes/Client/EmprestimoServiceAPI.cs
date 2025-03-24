using static LivrEtec.GIB.Controllers.EmprestimosController;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace LivrEtec.Testes.APIClient;

public sealed class EmprestimoServiceAPI(ILogger<EmprestimoServiceAPI> logger, HttpClient client) : IEmprestimoService
{
    private readonly ILogger<EmprestimoServiceAPI> logger = logger;
    private readonly HttpClient client = client;

    public async Task<int> Abrir(int idPessoa, int idlivro)
    {

        var response = await client.PostAsJsonAsync(
            "emprestimos",
            new RequestAbrirEmprestimo(idPessoa, idlivro)
        );
        response.EnsureSuccessStatusCode();
        return int.Parse(await response.Content.ReadAsStringAsync());

    }
    public Task<IEnumerable<Emprestimo>> Buscar(ParamBuscaEmprestimo parametros)
    {
        throw new NotImplementedException();
    }

        public Task<Emprestimo> Obter(int id)
    {
        throw new NotImplementedException();
    }
    public async Task Devolver(int idEmprestimo, bool? AtrasoJustificado = null, string? ExplicacaoAtraso = null)
    {

        var request = new RequestDevolverEmprestimo(idEmprestimo, AtrasoJustificado, ExplicacaoAtraso);
        await client.PatchAsJsonAsync("emprestimos/devolver", request);

    }
    public async Task Prorrogar(int idEmprestimo, DateTime novaData)
    {


        var request = new RequestProrrogarEmprestimo(idEmprestimo, novaData);
        await client.PatchAsJsonAsync("emprestimos/prorrogar", request);

    }

    public async Task RegistrarPerda(int idEmprestimo)
    {
        await client.PatchAsJsonAsync(
            "emprestimos/perda",
            new RequestPerdaEmprestimo(idEmprestimo)
        );

    }
    public async Task Excluir(int idEmprestimo)
    {
        await client.DeleteAsync($"emprestimos/{idEmprestimo}");
    }
}