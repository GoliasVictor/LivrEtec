namespace LivrEtec.GIB.Controllers;

[Route("api/emprestimos")]
[ApiController]
public sealed class EmprestimoController 
{
    private readonly ILogger<EmprestimoController> logger;
    private readonly IEmprestimoService emprestimoService;
    private readonly IIdentidadeService identidadeService;
    public EmprestimoController(ILogger<EmprestimoController> logger, IEmprestimoService emprestimoService, IIdentidadeService identidadeService)
    {
        this.logger = logger;
        this.emprestimoService = emprestimoService;
        this.identidadeService = identidadeService;
    }
    public record AbrirRequest(int IdPessoa, int IdLivro);
    [HttpPost()]
    public async Task<int> Abrir(AbrirRequest request)
    {
        return await emprestimoService.Abrir(request.IdPessoa, request.IdLivro);
    }
    public record BuscarRequest(
        int? IdLivro,
        int? IdPessoa,
        bool? Fechado,
        bool? Atrasado
    );
    [HttpGet("buscar")]
    public async Task<IEnumerable<DTO::Emprestimo>> Buscar([FromQuery] BuscarRequest request)
    {
        return (await emprestimoService.Buscar(new LEM::ParamBuscaEmprestimo(
            IdLivro: request.IdLivro,
            IdPessoa: request.IdPessoa,
            Fechado: request.Fechado,
            Atrasado: request.Atrasado
        ))).Select(e => (DTO::Emprestimo)e);
    }
    public record DevolverRequest(
        int IdEmprestimo,
        bool? AtrasoJustificado,
        string? ExplicacaoAtraso
    );
    [HttpPatch("devolver")]
    public async Task Devolver(DevolverRequest request)
    {
        await emprestimoService.Devolver(
            request.IdEmprestimo,
            request.AtrasoJustificado,
            request.ExplicacaoAtraso 
        );
    }
    public record ProrrogarRequest(
        int IdEmprestimo,
        DateTime NovaData 
    );
    [HttpPatch("prorrogar")]
    public async Task Prorrogar(ProrrogarRequest request)
    {
        await emprestimoService.Prorrogar(request.IdEmprestimo, request.NovaData);
    }
    public record PerdaRequest(
        int id
    );
    [HttpPatch("perda")]
    public async Task RegistrarPerda(PerdaRequest request)
    {
        await emprestimoService.RegistrarPerda(request.id);
    }
    [HttpDelete("{id}")]
    public async Task Excluir(int id)
    {
        await emprestimoService.Excluir(id);
    }

}
