using LivrEtec.GIB;
using Xunit.Abstractions;
using Grpc.Net.Client;

namespace LivrEtec.Testes.Remoto;

[Trait("Category", "Remoto")]
public sealed class TestesTagsServiceRPC: TestesTagsService<TagsServiceRPC>
{
	protected override TagsServiceRPC tagsService { get;init;}
    public TestesTagsServiceRPC(ITestOutputHelper output) 
		: base (  
			output,
			new BDUtilMySQl(Configuracao.StrConexaoMySQL, LogUtils.CreateLoggerFactory(output))
		)
	{
		GrpcChannel channel = gRPCUtil.GetGrpChannel(Configuracao.UrlGIBAPI);
        tagsService = new TagsServiceRPC(new GIB.RPC.Tags.TagsClient(channel),output.ToLogger<TagsServiceRPC>());
    }
}