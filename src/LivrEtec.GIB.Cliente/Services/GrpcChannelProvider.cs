using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec.GIB.Cliente.Services;
internal class GrpcChannelProvider
{
    IConfiguration Configuration;
    public GrpcChannelProvider(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public GrpcChannel GetGrpcChannel(string authToken)
    {
        var UrlAPI = Configuration["UrlAPI"] ?? throw new Exception("Endereço da API gRPC indefinido");

        var httpClient = new HttpClient();
        if (authToken is not null)
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

        var grpcChannelOptions = new GrpcChannelOptions {
            HttpClient = httpClient,
        };

        return GrpcChannel.ForAddress(UrlAPI, grpcChannelOptions);
    }
}
