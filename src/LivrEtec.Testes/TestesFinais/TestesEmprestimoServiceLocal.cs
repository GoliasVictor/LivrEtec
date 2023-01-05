using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
namespace LivrEtec.Testes.Local;

[Trait("Category", "Local")]
public sealed class TestesEmprestimoServiceLocal : TestesEmprestimoService<EmprestimoService>, IDisposable 
{
	readonly PacaContext BD;
	protected override EmprestimoService emprestimoService {get; init; }
	public TestesEmprestimoServiceLocal(ConfiguradorTestes configurador, ITestOutputHelper output) 
	: base ( 
		
		configurador,
		output,
		new RelogioStub(new DateTime(2022,1,1)),
		new BDUtilSqlLite(configurador.CreateLoggerFactory(output))
	)
	{
		BD = BDU.CriarContexto();
		var identidadeService = new IdentidadePermitidaStub(usuarioTeste);

		var repUsuarios = new RepUsuarios(BD, configurador.CreateLogger<RepUsuarios>(output));
		emprestimoService = new EmprestimoService(
			new RepEmprestimos(BD,repUsuarios, configurador.CreateLogger<RepEmprestimos>(output), relogio),
			new RepPessoas(BD, configurador.CreateLogger<RepPessoas>(output)),
			new RepLivros(BD, configurador.CreateLogger<RepLivros>(output)),
			identidadeService,
			relogio,
			configurador.CreateLogger<EmprestimoService>(output)
		);
	}

	public void Dispose()
	{
		BD.Dispose();
	}

}
