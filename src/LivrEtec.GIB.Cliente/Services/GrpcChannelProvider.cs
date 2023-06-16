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
    IConfiguracaoService Configuracao;
    string AuthToken;
    public void DefinirToken(string authToken)
    {
        AuthToken = authToken;
    }
    public GrpcChannelProvider(IConfiguracaoService configuration)
    {
        Configuracao = configuration;
    }
    public GrpcChannel GetGrpcChannel()
    {
        var UrlAPI = Configuracao["UrlAPI"] ?? throw new Exception("Endereço da API gRPC indefinido");

        var httpClient = new HttpClient();
        if (AuthToken is not null)
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthToken}");

        var grpcChannelOptions = new GrpcChannelOptions {
            HttpClient = httpClient,
        };

        return GrpcChannel.ForAddress(UrlAPI, grpcChannelOptions);
    }
}
