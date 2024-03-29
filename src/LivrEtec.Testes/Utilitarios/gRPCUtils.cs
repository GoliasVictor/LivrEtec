using Grpc.Core;
using Grpc.Net.Client;
using LivrEtec.GIB.Servidor.Services;

namespace LivrEtec.Testes.Utilitarios;

internal class gRPCUtil
{
    public static GrpcChannel GetGrpChannel(string? UrlAPI, Usuario usuario)
    {
        _ = UrlAPI ?? throw new Exception("Endereço da API gRPC indefinido");
        var credentials = CallCredentials.FromInterceptor((_, _) => Task.CompletedTask);


        var httpClient = new HttpClient();
        if (usuario is not null)
        {
            var token = TokenService.GerarToken(usuario.Id, Configuracao.AuthKey!);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        var grpcChannelOptions = new GrpcChannelOptions
        {

            HttpClient = httpClient,
        };


        return GrpcChannel.ForAddress(UrlAPI, grpcChannelOptions);
    }
}