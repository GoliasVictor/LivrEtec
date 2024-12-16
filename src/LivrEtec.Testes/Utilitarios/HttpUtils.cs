using LivrEtec.GIB.Services;

namespace LivrEtec.Testes.Utilitarios;

internal class HttpUtils
{
    public static HttpClient GetHttpClient(string? UrlAPI, Usuario usuario)
    {
        _ = UrlAPI ?? throw new Exception("Endereço da API indefinido");


        var httpClient = new HttpClient();
        if (usuario is not null)
        {
            var token = TokenService.GerarToken(usuario.Id, Configuracao.AuthKey!);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
		httpClient.BaseAddress = new Uri(UrlAPI);
        return httpClient;
    }
}