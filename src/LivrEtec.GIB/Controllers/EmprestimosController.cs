namespace LivrEtec.GIB.Controllers;

[Route("emprestimos")]
[ApiController]
public sealed class EmprestimosController 
{
    private readonly ILogger<EmprestimosController> logger;
    private readonly IEmprestimoService emprestimoService;
    private readonly IIdentidadeService identidadeService;
    public EmprestimosController(ILogger<EmprestimosController> logger, IEmprestimoService emprestimoService, IIdentidadeService identidadeService)
    {
        this.logger = logger;
        this.emprestimoService = emprestimoService;
        this.identidadeService = identidadeService;
    }
    public record RequestAbrirEmprestimo(int IdPessoa, int IdLivro);
    [HttpPost()]
    public async Task<int> Abrir(RequestAbrirEmprestimo request)
    {
        return await emprestimoService.Abrir(request.IdPessoa, request.IdLivro);
    }
    public record RequestBuscar(
        int? IdLivro,
        int? IdPessoa,
        bool? Fechado,
        bool? Atrasado
    );
    [HttpGet()]
    public async Task<IEnumerable<DTO::Emprestimo>> Buscar([FromQuery] RequestBuscar request)
    {
        return (await emprestimoService.Buscar(new LEM::ParamBuscaEmprestimo(
            IdLivro: request.IdLivro,
            IdPessoa: request.IdPessoa,
            Fechado: request.Fechado,
            Atrasado: request.Atrasado
        ))).Select(e => (DTO::Emprestimo)e);
    }
    public record RequestDevolverEmprestimo(
        int IdEmprestimo,
        bool? AtrasoJustificado,
        string? ExplicacaoAtraso
    );
    [HttpPatch("devolver")]
    public async Task Devolver(RequestDevolverEmprestimo request)
    {
        await emprestimoService.Devolver(
            request.IdEmprestimo,
            request.AtrasoJustificado,
            request.ExplicacaoAtraso 
        );
    }
    public record RequestProrrogarEmprestimo(
        int IdEmprestimo,
        DateTime NovaData 
    );
    [HttpPatch("prorrogar")]
    public async Task Prorrogar(RequestProrrogarEmprestimo request)
    {
        await emprestimoService.Prorrogar(request.IdEmprestimo, request.NovaData);
    }
    public record RequestPerdaEmprestimo(
        int id
    );
    [HttpPatch("perda")]
    public async Task RegistrarPerda(RequestPerdaEmprestimo request)
    {
        await emprestimoService.RegistrarPerda(request.id);
    }
    [HttpDelete("{id}")]
    public async Task Excluir(int id)
    {
        await emprestimoService.Excluir(id);
    }

}
