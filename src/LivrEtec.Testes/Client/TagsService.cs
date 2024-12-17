using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
namespace LivrEtec.Testes.Client;

public sealed class TagsServiceAPI : ITagsService
{
    private readonly ILogger<TagsServiceAPI> logger;
    private readonly HttpClient client;
    public TagsServiceAPI(HttpClient client, ILogger<TagsServiceAPI> logger)
    {
        this.client = client;
        this.logger = logger;
    }
    public async Task<int> Registrar(Tag tag)
    {
        Validador.ErroSeInvalido(tag);
        var response = await client.PostAsJsonAsync("tags", tag);
        response.EnsureSuccessStatusCode();
        return Int32.Parse(await response.Content.ReadAsStringAsync());
    }

    public async Task Editar(Tag tag)
    {
        _ = tag ?? throw new ArgumentNullException(nameof(tag));
        _ = await client.PutAsJsonAsync("tags", (DTO::Tag)tag);
    }

    public async Task<Tag?> Obter(int id)
    {
        return await client.GetFromJsonAsync<DTO::Tag?>($"tags/{id}");
    }


    public async Task Remover(int id)
    {
        _ = await client.DeleteAsync($"tags/{id}");
    }

    public async Task<IEnumerable<Tag>> Buscar(string nome)
    {
        nome ??= "";
        var uri = QueryHelpers.AddQueryString("tags", new Dictionary<string, string?> {
                {nameof(nome), nome}
            });
        return (await client.GetFromJsonAsync<List<DTO::Tag>>(uri))
                .Select(l => (Tag)l!);
    }

}
