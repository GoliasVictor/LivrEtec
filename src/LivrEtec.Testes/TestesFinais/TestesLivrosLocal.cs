using Xunit.Abstractions;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes.Local;

[Trait("Category", "Local")]
public sealed class TestesLivrosLocal : TestesLivro<RepLivros>, IDisposable 
{
	readonly AcervoService acervoService;
	readonly PacaContext BD;
	protected override  RepLivros RepLivros {get; init; }
	public TestesLivrosLocal(ConfiguradorTestes configurador, ITestOutputHelper output) 
	: base(
		configurador, 
		output, 
		new BDUtilSqlLite(configurador.CreateLoggerFactory(output))
	)
	{
		BD = BDU.CriarContexto();
		acervoService = new AcervoService(BDU.CriarContexto(), configurador.CreateLoggerFactory(output).CreateLogger<AcervoService>(), new RelogioStub(new DateTime(2000,1,1)));
		RepLivros = (RepLivros)acervoService.Livros ;
	}

	public void Dispose()
	{
		BD.Dispose();
	}

}
