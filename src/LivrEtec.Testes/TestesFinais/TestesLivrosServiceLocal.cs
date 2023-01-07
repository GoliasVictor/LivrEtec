using Xunit.Abstractions;

using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes.Local;

[Trait("Category", "Local")]
public sealed class TestesLivrosLocal : TestesLivrosService<LivrosService>, IDisposable 
{
	readonly PacaContext BD;
	protected override  LivrosService livrosService {get; init; }
	public TestesLivrosLocal(ITestOutputHelper output) 
	: base(
		output, 
		new BDUtilSqlLite(LogUtils.CreateLoggerFactory(output))
	)
	{
		BD = BDU.CriarContexto(); 
		livrosService = new LivrosService(
			new RepLivros(BD, LogUtils.CreateLogger<RepLivros>(output)),
			new IdentidadePermitidaStub(),
			LogUtils.CreateLogger<LivrosService>(output)
		);
	}

	public void Dispose()
	{
		BD.Dispose();
	}

}
