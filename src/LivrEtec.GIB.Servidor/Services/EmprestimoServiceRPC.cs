using Grpc.Core;
using LivrEtec.GIB.RPC;
using RPC = LivrEtec.GIB.RPC;
using static LivrEtec.GIB.RPC.Emprestimo.Types;

namespace LivrEtec.GIB.Servidor;

public sealed class EmprestimoServiceRPC : Emprestimos.EmprestimosBase
{
	readonly ILogger<EmprestimoServiceRPC> logger;
	readonly IEmprestimoService emprestimoService;
	readonly IIdentidadeService identidadeService;
	public EmprestimoServiceRPC(ILogger<EmprestimoServiceRPC> logger, IEmprestimoService emprestimoService, IIdentidadeService identidadeService)
	{
		this.logger = logger;
		this.emprestimoService = emprestimoService;
		this.identidadeService = identidadeService;
	}

	public override async Task<IdEmprestimo> Abrir(AbrirRequest request, ServerCallContext context)
	{
		return new IdEmprestimo()
		{
			Id = await emprestimoService.Abrir(request.IdPessoa, request.IdLivro)
		};
	}

	public override async Task<ListaEmprestimos> Buscar(BuscarRequest request, ServerCallContext context)
	{
		IEnumerable<Emprestimo> Emprestimos = await emprestimoService.Buscar(new ParamBuscaEmprestimo(
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
	public override async Task<Empty> Devolver(DevolverRequest request, ServerCallContext context)
	{
		await emprestimoService.Devolver(
			request.IdEmprestimo,
			request.HasAtrasoJustificado ? request.AtrasoJustificado : null,
			request.HasExplicacaoAtraso ? request.ExplicacaoAtraso : null
		);
		return new Empty();
	}

	public override async Task<Empty> Prorrogar(ProrrogarRequest request, ServerCallContext context)
	{
		await emprestimoService.Prorrogar(request.IdEmprestimo, request.NovaData.ToDateTime());
		return new Empty();
	}

	public override async Task<Empty> RegistrarPerda(IdEmprestimo request, ServerCallContext context)
	{
		await emprestimoService.RegistrarPerda(request.Id);
		return new Empty();
	}

	public override async Task<Empty> Excluir(IdEmprestimo request, ServerCallContext context)
	{
		await emprestimoService.Excluir(request.Id);
		return new Empty();
	}

}
