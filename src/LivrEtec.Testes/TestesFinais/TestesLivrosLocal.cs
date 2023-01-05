using Xunit.Abstractions;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes.Local;

[Trait("Category", "Local")]
public sealed class TestesLivrosLocal : TestesLivro<RepLivros>, IDisposable 
{
	readonly PacaContext BD;
	protected override  RepLivros RepLivros {get; init; }
	public TestesLivrosLocal(ITestOutputHelper output) 
	: base(
		output, 
		new BDUtilSqlLite(LogUtils.CreateLoggerFactory(output))
	)
	{
		BD = BDU.CriarContexto(); 
		RepLivros = new RepLivros(BD, LogUtils.CreateLogger<RepLivros>(output)) ;
	}

	public void Dispose()
	{
		BD.Dispose();
	}

}
