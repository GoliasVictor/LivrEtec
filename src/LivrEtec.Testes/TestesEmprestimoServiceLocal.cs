
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LivrEtec.Testes;

[Trait("Category", "Local")]
public class TestesEmprestimoServiceLocal : TestesEmprestimoService<EmprestimoService>, IDisposable 
{
	readonly PacaContext BD;
	protected override EmprestimoService emprestimoService {get; init; }
            
	public TestesEmprestimoServiceLocal(ConfiguradorTestes configurador, ITestOutputHelper output) : base(configurador, output, new RelogioStub(new DateTime(2022,1,1)))
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
