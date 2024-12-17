using LivrEtec.Models;
using Microsoft.AspNetCore.WebUtilities;
namespace LivrEtec.GIB.Services.Cliente;

public sealed class TagsService : ITagsService
{
    private readonly ILogger<TagsService> logger;
    private readonly HttpClient client;
    public TagsService(HttpClient client, ILogger<TagsService> logger)
    {
        this.client = client;
        this.logger = logger;
    }
    public async Task<int> Registrar(Tag tag)
    {
        Validador.ErroSeInvalido(tag);
        var response = await client.PostAsJsonAsync("/api/tags", tag);
        response.EnsureSuccessStatusCode();
        return Int32.Parse(await response.Content.ReadAsStringAsync());



    }

    public async Task Editar(Tag tag)
    {
        _ = tag ?? throw new ArgumentNullException(nameof(tag));
        _ = await client.PutAsJsonAsync("api/tags", (DTO::Tag)tag);
    }

    public async Task<Tag?> Obter(int id)
    {
        return await client.GetFromJsonAsync<DTO::Tag?>($"api/tags/{id}");
    }


    public async Task Remover(int id)
    {
        _ = await client.DeleteAsync($"api/tags/{id}");
    }

    public async Task<IEnumerable<Tag>> Buscar(string nome)
    {
        nome ??= "";
        var uri = QueryHelpers.AddQueryString("api/tags", new Dictionary<string, string?> {
                {nameof(nome), nome}
            });
        return (await client.GetFromJsonAsync<List<DTO::Tag>>(uri))
                .Select(l => (Tag)l!);
    }

}
