using LivrEtec.Exceptions;
using LivrEtec.Services;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.Services;

public sealed class EmprestimoService : IEmprestimoService
{
    private readonly IRelogio relogio;
    private readonly ILogger<EmprestimoService>? Logger;
    private readonly IIdentidadeService identidadeService;
    private readonly IRepEmprestimos repEmprestimos;
    private readonly IRepPessoas repPessoas;
    private readonly IRepLivros repLivros;
    public EmprestimoService(
        IRepEmprestimos repEmprestimos,
        IRepPessoas repPessoas,
        IRepLivros repLivros,
        IIdentidadeService identidadeService,
        IRelogio relogio,
        ILogger<EmprestimoService>? logger
    )
    {
        this.repEmprestimos = repEmprestimos;
        this.repPessoas = repPessoas;
        this.repLivros = repLivros;
        this.identidadeService = identidadeService;
        this.relogio = relogio;
        Logger = logger;
    }

    public async Task<int> Abrir(int idPessoa, int idLivro)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Emprestimo.Criar);
        await identidadeService.CarregarUsuario();

        Pessoa pessoa = await repPessoas.ObterObter(idPessoa)
            ?? throw new InvalidOperationException($"Pessoa de id {{{idPessoa}}} não existe.");
        Livro livro = await repLivros.Obter(idLivro)
            ?? throw new InvalidOperationException($"Livro de id {{{idPessoa}}} não existe.");

        var QtEmprestada = await repEmprestimos.ObterQuantidadeLivrosEmprestado(idLivro);
        var LivrosDisponiveis = livro.Quantidade - QtEmprestada;
        if (LivrosDisponiveis <= 0)
        {
            throw new LivroEsgotadoException(livro.Id, $"Não é possivel abrir emprestimo, livro {{{livro.Id}}}");
        }

        var Emprestimo = new Emprestimo()
        {
            Pessoa = pessoa,
            Livro = livro,
            UsuarioCriador = await identidadeService.ObterUsuario(),
            DataEmprestimo = relogio.Agora,
            FimDataEmprestimo = relogio.Agora.AddDays(30),
        };
        var id = await repEmprestimos.Registrar(Emprestimo);
        Logger?.LogInformation("Emprestimo {{{id}}} aberto", id);
        return id;
    }

    public async Task<IEnumerable<Emprestimo>> Buscar(ParamBuscaEmprestimo parametros)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Emprestimo.Visualizar);
        IEnumerable<Emprestimo> emprestimos = await repEmprestimos.Buscar(parametros);
        Logger?.LogInformation("Foram buscado os emprestimso do usuario {{{idPessoa}}} e livro {{{idLivro}}}", parametros.IdPessoa, parametros.IdLivro);
        return emprestimos;
    }

    public async Task Prorrogar(int idEmprestimo, DateTime novaData)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Emprestimo.Editar);
        await repEmprestimos.EditarFimData(idEmprestimo, novaData);
        Logger?.LogInformation("Data do livro {{{idEmprestimo}}}, foi modificada para {{{novaData}}}", idEmprestimo, novaData);

    }
    public Task Devolver(int idEmprestimo, bool? AtrasoJustificado = null, string? ExplicacaoAtraso = null)
    {
        return FecharAsync(new ParamFecharEmprestimo()
        {
            IdEmprestimo = idEmprestimo,
            Devolvido = true,
            AtrasoJustificado = AtrasoJustificado,
            ExplicacaoAtraso = ExplicacaoAtraso
        });
    }
    public async Task RegistrarPerda(int idEmprestimo)
    {
        await FecharAsync(new ParamFecharEmprestimo()
        {
            IdEmprestimo = idEmprestimo,
            Devolvido = false,
        });
        var livro = (await repEmprestimos.Obter(idEmprestimo))!.Livro.Clone();
        livro.Quantidade -= 1;
        await repLivros.Editar(livro);
    }
    private async Task FecharAsync(ParamFecharEmprestimo parametros)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Emprestimo.Fechar);
        parametros.idUsuarioFechador = (int)identidadeService.IdUsuario;
        await repEmprestimos.Fechar(parametros);

        Logger?.LogInformation("O emprestimo {{{idEmprestimo}}} foi devolvido", parametros.IdEmprestimo);
    }

    public async Task Excluir(int idEmprestimo)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Emprestimo.Excluir);
        await repEmprestimos.Excluir(idEmprestimo);

        Logger?.LogInformation("O emprestimo {{{idEmprestimo}}} foi excluido", idEmprestimo);
    }
}
