using LivrEtec.GIB.RPC;
using static LivrEtec.GIB.RPC.Emprestimo.Types;
namespace LivrEtec.GIB.Services;

[Route("api/emprestimos")]
[ApiController]
public sealed class EmprestimoServiceRPC 
{
    private readonly ILogger<EmprestimoServiceRPC> logger;
    private readonly IEmprestimoService emprestimoService;
    private readonly IIdentidadeService identidadeService;
    public EmprestimoServiceRPC(ILogger<EmprestimoServiceRPC> logger, IEmprestimoService emprestimoService, IIdentidadeService identidadeService)
    {
        this.logger = logger;
        this.emprestimoService = emprestimoService;
        this.identidadeService = identidadeService;
    }
    [HttpPost()]
    public async Task<IdEmprestimo> Abrir(AbrirRequest request)
    {
        return new IdEmprestimo()
        {
            Id = await emprestimoService.Abrir(request.IdPessoa, request.IdLivro)
        };
    }
    [HttpGet("buscar")]
    public async Task<ListaEmprestimos> Buscar(BuscarRequest request)
    {
        IEnumerable<LEM::Emprestimo> Emprestimos = await emprestimoService.Buscar(new LEM::ParamBuscaEmprestimo(
            IdLivro: request.IdLivro,
            IdPessoa: request.IdPessoa,
            Fechado: request.Fechado,
            Atrasado: request.Atrasado
        ));
        return new ListaEmprestimos()
        {
            Emprestimos = { Emprestimos.Select(l => (RPC::Emprestimo)l).ToArray() }
        };
    }
    [HttpPatch("devolver")]
    public async Task<Empty> Devolver(DevolverRequest request)
    {
        await emprestimoService.Devolver(
            request.IdEmprestimo,
            request.HasAtrasoJustificado ? request.AtrasoJustificado : null,
            request.HasExplicacaoAtraso ? request.ExplicacaoAtraso : null
        );
        return new Empty();
    }
    [HttpPatch("prorrogar")]
    public async Task<Empty> Prorrogar(ProrrogarRequest request)
    {
        await emprestimoService.Prorrogar(request.IdEmprestimo, request.NovaData.ToDateTime());
        return new Empty();
    }

    [HttpPatch("perda")]
    public async Task<Empty> RegistrarPerda(IdEmprestimo request)
    {
        await emprestimoService.RegistrarPerda(request.Id);
        return new Empty();
    }
    [HttpDelete()]
    public async Task<Empty> Excluir(IdEmprestimo request)
    {
        await emprestimoService.Excluir(request.Id);
        return new Empty();
    }

}
