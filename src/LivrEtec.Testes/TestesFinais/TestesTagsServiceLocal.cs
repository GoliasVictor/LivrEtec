using Xunit.Abstractions;

namespace LivrEtec.Testes.TestesFinais;

[Trait("Category", "Local")]
public sealed class TestesTagsLocal : TestesTagsService<TagsService>, IDisposable
{
    private readonly PacaContext BD;
    protected override TagsService tagsService { get; init; }
    public TestesTagsLocal(ITestOutputHelper output)
    : base(
        output,
        new BDUtilSqlLite(LogUtils.CreateLoggerFactory(output))
    )
    {
        BD = BDU.CriarContexto();
        tagsService = new TagsService(
            new RepTags(BD, LogUtils.CreateLogger<RepTags>(output)),
            new IdentidadePermitidaStub(),
            LogUtils.CreateLogger<TagsService>(output)
        );
    }

    public void Dispose()
    {
        BD.Dispose();
    }

}
