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
		var acervoService = new AcervoService(BD, configurador.CreateLogger<AcervoService>(output), relogio);
		var identidadeService = new IdentidadePermitidaStub(usuarioTeste);
		emprestimoService = new EmprestimoService(
			acervoService,
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
