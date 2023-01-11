using Grpc.Core;
using LivrEtec.GIB.RPC;
using RPC = LivrEtec.GIB.RPC;
using static LivrEtec.GIB.RPC.Emprestimo.Types;

namespace LivrEtec.GIB.Servidor;

public sealed class EmprestimoServiceRPC : Emprestimos.EmprestimosBase
{
	readonly ILogger<EmprestimoServiceRPC> logger;
	readonly IEmprestimoService emprestimoService;
	readonly IdentidadeServiceRPC? identidadeService;
	public EmprestimoServiceRPC(ILogger<EmprestimoServiceRPC> logger, IEmprestimoService emprestimoService, IIdentidadeService identidadeService)
	{
		this.logger = logger;
		this.emprestimoService = emprestimoService;
		this.identidadeService = identidadeService as IdentidadeServiceRPC;
	}

	public override async Task<IdEmprestimo> Abrir(AbrirRequest request, ServerCallContext context)
	{
		identidadeService?.DefinirContexto(context);
		return new IdEmprestimo()
		{
			Id = await emprestimoService.AbrirAsync(request.IdPessoa, request.IdLivro)
		};
	}

	public override async Task<ListaEmprestimos> Buscar(BuscarRequest request, ServerCallContext context)
	{
		identidadeService?.DefinirContexto(context);
		IEnumerable<Emprestimo> Emprestimos = await emprestimoService.BuscarAsync(new ParamBuscaEmprestimo(
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
		identidadeService?.DefinirContexto(context);
		await emprestimoService.DevolverAsync(
			request.IdEmprestimo,
			request.HasAtrasoJustificado ? request.AtrasoJustificado : null,
			request.HasExplicacaoAtraso ? request.ExplicacaoAtraso : null
		);
		return new Empty();
	}

	public override async Task<Empty> Prorrogar(ProrrogarRequest request, ServerCallContext context)
	{
		identidadeService?.DefinirContexto(context);
		await emprestimoService.ProrrogarAsnc(request.IdEmprestimo, request.NovaData.ToDateTime());
		return new Empty();
	}

	public override async Task<Empty> RegistrarPerda(IdEmprestimo request, ServerCallContext context)
	{
		identidadeService?.DefinirContexto(context);
		await emprestimoService.RegistrarPerdaAsync(request.Id);
		return new Empty();
	}

	public override async Task<Empty> Excluir(IdEmprestimo request, ServerCallContext context)
	{
		identidadeService?.DefinirContexto(context);
		await emprestimoService.ExcluirAsync(request.Id);
		return new Empty();
	}

}
