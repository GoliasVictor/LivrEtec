using LivrEtec.Models;
using Microsoft.Extensions.Logging;

namespace LivrEtec.GIB.Services;

public sealed class LivrosServiceRPC : ILivrosService
{
    private readonly ILogger<LivrosServiceRPC> logger;
    private readonly RPC::Livros.LivrosClient livrosClientRPC;
    public LivrosServiceRPC(RPC::Livros.LivrosClient livrosClientRPC, ILogger<LivrosServiceRPC> logger)
    {
        this.livrosClientRPC = livrosClientRPC;
        this.logger = logger;
    }

    public async Task Editar(Livro livro)
    {
        _ = livro ?? throw new ArgumentNullException(nameof(livro));
        if (livro.Tags.Any((t) => t is null))
        {
            throw new InvalidDataException("tag nula");
        }

        livro.Tags ??= new();
        try
        {
            _ = await livrosClientRPC.EditarAsync(livro);
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }

    public async Task<Livro?> Obter(int id)
    {
        try
        {
            return await livrosClientRPC.ObterAsync(new RPC::Id(id));
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }

    public async Task Registrar(Livro livro)
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
            _ = await livrosClientRPC.RegistrarAsync(livro);
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
            _ = await livrosClientRPC.RemoverAsync(new RPC::Id(id));
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }

    public async Task<IEnumerable<Livro>> Buscar(string nome, string nomeAutor, IEnumerable<int>? idTags)
    {
        nome ??= "";
        nomeAutor ??= "";
        idTags ??= new List<int>();
        try
        {
            RPC::ListaLivros listaLivros = await livrosClientRPC.BuscarAsync(new RPC::ParamBusca() { NomeLivro = nome, NomeAutor = nomeAutor, IdTags = { idTags } });
            return listaLivros.Livros.Select(l => (Livro)l!);
        }
        catch (RpcException ex)
        {
            throw ManipuladorException.RpcExceptionToException(ex);
        }
    }

}