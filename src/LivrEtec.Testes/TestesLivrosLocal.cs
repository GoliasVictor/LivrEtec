
namespace LivrEtec.Testes;

public class TestesLivrosLocal : TestesLivro<RepLivros>, IDisposable 
{
	AcervoService acervoService;
	PacaContext BD;
	protected override  RepLivros RepLivros => (RepLivros)acervoService.Livros;
	public TestesLivrosLocal(ConfiguradorTestes configurador) : base(configurador)
	{
		BD = BDU.CriarContexto();
		acervoService = new AcervoService(BDU.PacaContextFactory, null);
	}
	
	public void Dispose(){
		BD.Dispose();
	}

}
