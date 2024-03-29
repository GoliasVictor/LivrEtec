using Xunit.Abstractions;


namespace LivrEtec.Testes;

[Collection("UsaBancoDeDados")]
public abstract class TestesTagsService<T> where T : ITagsService
{
	private const int ID_TAG_EXISTENTE = 1;
	protected abstract T tagsService { get; init; }
	protected readonly BDUtil BDU;

	public static void AssertEhIgual<K>(IEnumerable<K> A, IEnumerable<K> B)
	{
		Assert.Equal(new HashSet<K>(A), new HashSet<K>(B));
	}

	private static void AssertTagIgual(Tag tagEsperada, Tag tagAtual)
	{
		Assert.Equal(tagEsperada.Nome, tagAtual.Nome);
		Assert.Equal(tagEsperada.Id, tagAtual.Id);
	}
	public TestesTagsService(ITestOutputHelper output, BDUtil bdu)
	{
		BDU = bdu;
		BDU.Tags = new Tag[]{
			new Tag(ID_TAG_EXISTENTE,"Aventura"),
			new Tag(2,"Fantasia"),
			new Tag(3,"Politica"),
			new Tag(4,"Literatura"),
			new Tag(5,"Sociologia"),
		};
		BDU.SalvarDados();
	}
	[Fact]
	public async Task Registrar_Valida()
	{
		var tagEsperada = new Tag
		{
			Nome = "Matématica",
		};
		var id = await tagsService.Registrar(tagEsperada);
		_ = await BDU.gTagBanco(id);
	}
	[Fact]
	public async Task Obter_Valida()
	{
		var idTag = ID_TAG_EXISTENTE;
		Tag tagEsperada = BDU.gTag(idTag);

		Tag? tagAtual = await tagsService.Obter(idTag);
		Assert.NotNull(tagAtual);
		AssertTagIgual(tagEsperada, tagAtual!);
	}
	[Fact]
	public async Task Buscar_Valida()
	{
		var nomeBusca = "li";
		Tag[] tagsEsperadas = new[] {
			new Tag(3,"Politica"),
			new Tag(4,"Literatura"),
		};

		IEnumerable<Tag> tagsAtuais = await tagsService.Buscar(nomeBusca);

		Assert.NotNull(tagsAtuais);
		AssertEhIgual(tagsEsperadas, tagsAtuais);
	}

	[Fact]
	public async Task Editar_Valida()
	{
		var idTag = ID_TAG_EXISTENTE;
		var tagEsperada = new Tag
		{
			Id = idTag,
			Nome = "Aventura Medieval"
		};

		await tagsService.Editar(tagEsperada);

		Tag? tagAtual = await BDU.gTagBanco(idTag);

		Assert.NotNull(tagAtual);
		AssertTagIgual(tagEsperada, tagAtual!);
	}

	[Fact]
	public async Task Excluir_Valida()
	{
		var idTag = ID_TAG_EXISTENTE;

		await tagsService.Remover(idTag);

		Tag? tagAtual = await BDU.gTagBanco(idTag);
		Assert.Null(tagAtual);
	}
}
