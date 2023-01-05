using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
namespace LivrEtec.Testes.Local;

[Trait("Category", "Local")]
public sealed class TestesEmprestimoServiceLocal : TestesEmprestimoService<EmprestimoService>, IDisposable 
{
	readonly PacaContext BD;
	protected override EmprestimoService emprestimoService {get; init; }
	public TestesEmprestimoServiceLocal(ITestOutputHelper output) 
	: base ( 
		output,
		new RelogioStub(new DateTime(2022,1,1)),
		new BDUtilSqlLite(LogUtils.CreateLoggerFactory(output))
	)
	{
		var loggerFactory = LogUtils.CreateLoggerFactory(output);
		BD = BDU.CriarContexto();
		var identidadeService = new IdentidadePermitidaStub(usuarioTeste);

		var repUsuarios = new RepUsuarios(BD, loggerFactory.CreateLogger<RepUsuarios>());
		emprestimoService = new EmprestimoService(
			new RepEmprestimos(BD,repUsuarios, loggerFactory.CreateLogger<RepEmprestimos>(), relogio),
			new RepPessoas(BD, loggerFactory.CreateLogger<RepPessoas>()),
			new RepLivros(BD, loggerFactory.CreateLogger<RepLivros>()),
			identidadeService,
			relogio,
			loggerFactory.CreateLogger<EmprestimoService>()
		);
	}

	public void Dispose()
	{
		BD.Dispose();
	}

}
