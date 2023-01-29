using LivrEtec.Servidor.BD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.Repositorios;

public class RepEmprestimos : Repositorio, IRepEmprestimos
{
    private readonly IRelogio _relogio;
    private readonly IRepUsuarios repUsuarios;
    public RepEmprestimos(PacaContext BD, IRepUsuarios repUsuarios, ILogger<RepEmprestimos> logger, IRelogio relogio) : base(BD, logger)
    {
        this.repUsuarios = repUsuarios;
        _relogio = relogio;
    }
    public async Task<int> Registrar(Emprestimo emprestimo)
    {
        Validador.ErroSeInvalido(emprestimo);
        _ = BD.Attach(emprestimo);
        BD.Entry(emprestimo).State = EntityState.Added;
        _ = await BD.SaveChangesAsync();
        return emprestimo.Id;

    }
    public async Task<int> ObterQuantidadeLivrosEmprestado(int idLivro)
    {
        return await BD.Emprestimos.CountAsync((e) => e.Livro.Id == idLivro && !e.Fechado);
    }
    public async Task<IEnumerable<Emprestimo>> Buscar(ParamBuscaEmprestimo parametros)
    {
		IQueryable<Emprestimo> emprestimos = from emprestimo in BD.Emprestimos
											 where parametros.Fechado != null || emprestimo.Fechado == parametros.Fechado
											 where parametros.IdPessoa != null || emprestimo.Pessoa.Id == parametros.IdPessoa
											 where parametros.IdLivro != null || emprestimo.Livro.Id == parametros.IdLivro
                                             let atrasado = emprestimo.Fechado ? emprestimo.FimDataEmprestimo > _relogio.Agora
                                                                               : emprestimo.Devolvido == true && emprestimo.FimDataEmprestimo > emprestimo.DataFechamento
											 where parametros.Atrasado != null || parametros.Atrasado == atrasado
		select emprestimo;
		return await emprestimos.ToListAsync();
	}

	public async Task<Emprestimo?> Obter(int idEmprestimo)
    {

        Emprestimo? emprestimo = await BD.Emprestimos.FindAsync(idEmprestimo);
        if (emprestimo == null)
        {
            return emprestimo;
        }

        await BD.Entry(emprestimo).Reference(l => l.Pessoa).LoadAsync();
        await BD.Entry(emprestimo).Reference(l => l.Livro).LoadAsync();
        return emprestimo;
    }
    public async Task EditarFimData(int idEmprestimo, DateTime NovaData)
    {
        Emprestimo emprestimo = await Obter(idEmprestimo)
            ?? throw new InvalidOperationException($"Emprestimo {idEmprestimo} não existe");
        emprestimo.FimDataEmprestimo = NovaData;
        _ = BD.Update(emprestimo);
        _ = await BD.SaveChangesAsync();
    }
    public async Task Excluir(int idEmprestimo)
    {
        Emprestimo emprestimo = await Obter(idEmprestimo)
            ?? throw new InvalidOperationException($"Emprestimo {idEmprestimo} não existe");
        _ = BD.Remove(emprestimo);
        _ = await BD.SaveChangesAsync();
    }

    public async Task Fechar(ParamFecharEmprestimo parametros)
    {
        Emprestimo emprestimo = await Obter(parametros.IdEmprestimo)
            ?? throw new InvalidOperationException($"Não existe o emprestimo de id {parametros.IdEmprestimo}");
        Usuario UsuarioFechador = await repUsuarios.Obter(parametros.idUsuarioFechador)
            ?? throw new InvalidOperationException($"Não é possivel fechar emprestimo porque usuario de id {{{parametros.idUsuarioFechador}}} não existe.");

        emprestimo.Fechado = true;
        emprestimo.DataFechamento = _relogio.Agora;
        emprestimo.Devolvido = parametros.Devolvido;
        emprestimo.UsuarioFechador = UsuarioFechador;
        if (parametros.Devolvido)
        {
            emprestimo.AtrasoJustificado = parametros.AtrasoJustificado;
            emprestimo.ExplicacaoAtraso = parametros.ExplicacaoAtraso;
        }

        _ = BD.SaveChanges();
    }
}
