using Grpc.Net.Client;

namespace LivrEtec.Testes;
 
class gRPCUtil {
	static public GrpcChannel GetGrpChannel(string? UrlAPI){
 		_ = UrlAPI?? throw new Exception("Endereço da API gRPC indefinido");
        var httpClient = new HttpClient();
		httpClient.DefaultRequestHeaders.Add("id",(2).ToString());
		var grpcChannelOptions  = new GrpcChannelOptions(){
			Credentials = Grpc.Core.ChannelCredentials.Insecure,
			HttpClient = httpClient
		};
        return GrpcChannel.ForAddress(UrlAPI, grpcChannelOptions);

	}	
}