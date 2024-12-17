using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes.APIClient;


public sealed class LivrosServiceAPI : ILivrosService
{
    private readonly ILogger<LivrosServiceAPI> logger;
    private readonly HttpClient client;
    public LivrosServiceAPI(HttpClient client, ILogger<LivrosServiceAPI> logger)
    {
        this.client = client;
        this.logger = logger;
    }

    public async Task Editar(LEM::Livro livro)
    {
        _ = livro ?? throw new ArgumentNullException(nameof(livro));
        if (livro.Tags.Any((t) => t is null))
        {
            throw new InvalidDataException("tag nula");
        }

        livro.Tags ??= new();

        _ = await client.PutAsJsonAsync("api/livros", (DTO::Livro)livro);

    }

    public async Task<LEM::Livro?> Obter(int id)
    {

        return await client.GetFromJsonAsync<DTO::Livro>($"api/livros/{id}");

    }

    public async Task Registrar(LEM::Livro livro)
    {
        if (livro is not null)
        {
            livro.Tags ??= new();
            livro.Autores ??= new();
        }
        Validador.ErroSeInvalido(livro);
        if (string.IsNullOrWhiteSpace(livro.Nome) || livro.Id < 0)
        {
            throw new InvalidDataException();
        }
        _ = await client.PostAsJsonAsync("api/livros", (DTO::Livro)livro);
    }


    public async Task Remover(int id)
    {
        _ = await client.DeleteAsync($"api/livros/{id}");
    }

    public async Task<IEnumerable<LEM::Livro>> Buscar(string nome, string nomeAutor, IEnumerable<int>? idTags)
    {
        nome ??= "";
        nomeAutor ??= "";
        idTags ??= new List<int>();

        var uri = "/api/livros";
        uri = QueryHelpers.AddQueryString(uri, "NomeLivro", nome);
        uri = QueryHelpers.AddQueryString(uri, "NomeAutor", nomeAutor);
        foreach (var t in idTags)
            uri = QueryHelpers.AddQueryString(uri, "IdTags", t.ToString());
        Console.WriteLine(uri);
        var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        Console.WriteLine(response.Content.ReadAsStringAsync());
        return (await response.Content.ReadFromJsonAsync<IEnumerable<DTO::Livro>>())
            .Select(l => (LEM::Livro)l!);
    }

}