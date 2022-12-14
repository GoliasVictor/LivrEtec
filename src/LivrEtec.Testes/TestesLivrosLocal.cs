
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LivrEtec.Testes;

[Trait("Category", "Local")]
public class TestesLivrosLocal : TestesLivro<RepLivros>, IDisposable 
{
	readonly AcervoService acervoService;
	readonly PacaContext BD;
	protected override  RepLivros RepLivros => (RepLivros)acervoService.Livros;
	public TestesLivrosLocal(ConfiguradorTestes configurador, ITestOutputHelper output) : base(configurador, output)
	{
		BD = BDU.CriarContexto();
		acervoService = new AcervoService(BDU.CriarContexto(), configurador.CreateLoggerFactory(output).CreateLogger<AcervoService>(), new RelogioStub(new DateTime(2000,1,1)));
	}

	public void Dispose()
	{
		BD.Dispose();
	}

}
