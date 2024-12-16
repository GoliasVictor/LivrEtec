using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace LivrEtec.GIB.Services.Cliente;

public sealed class LivrosServiceRPC : ILivrosService
{
    private readonly ILogger<LivrosServiceRPC> logger;
    private readonly HttpClient client;
    public LivrosServiceRPC(HttpClient client, ILogger<LivrosServiceRPC> logger)
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
        try
        {
            _ = await client.PutAsJsonAsync("api/livros", (RPC::Livro)livro);
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }

    public async Task<LEM::Livro?> Obter(int id)
    {
        try
        {
            return await client.GetFromJsonAsync<RPC::Livro>($"api/livros/{id}");
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
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

        try
        {
            _ = await client.PostAsJsonAsync("api/livros", (RPC::Livro)livro);
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }

    }


    public async Task Remover(int id)
    {
        try
        {
            _ = await client.DeleteAsync($"api/livros/{id}");
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }

    public async Task<IEnumerable<LEM::Livro>> Buscar(string nome, string nomeAutor, IEnumerable<int>? idTags)
    {
        nome ??= "";
        nomeAutor ??= "";
        idTags ??= new List<int>();
        try
        {


            var uri = "/api/livros";
            
            uri = QueryHelpers.AddQueryString(uri, "NomeLivro", nome);
            uri = QueryHelpers.AddQueryString(uri, "NomeAutor", nomeAutor);
            foreach(var t in idTags)
                uri = QueryHelpers.AddQueryString(uri, "IdTags", t.ToString());
            Console.WriteLine(uri);
            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync());
            return (await response.Content.ReadFromJsonAsync<IEnumerable<RPC::Livro>>())
                .Select(l => (LEM::Livro)l!);
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }

} 