using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes;
public abstract class TestesBD : IClassFixture<ConfiguradorTestes>, IDisposable
{
	public PacaContext BD;
	protected ConfiguracaoTeste configuracao;
	public ILoggerFactory loggerFactory = LoggerFactory.Create((lb)=> { 
		lb.AddConsole();
		lb.AddFilter((_,_, logLevel)=> logLevel >= LogLevel.Information);
	});
	protected TestesBD(ConfiguradorTestes configurador)
	{
		configuracao = configurador.Config;
		BD = new PacaContext(configurador.Config, loggerFactory);
	}


	T[] AddRange<T>(T[] entities)
	{
		foreach (var entity in entities)
			BD.Add(entity!);
		return entities.ToArray();
	}
	private Tag[] tags = new Tag[0];
	private Livro[] livros =  new Livro[0];
	private Autor[] autores =  new Autor[0];
	private Pessoa[] pessoas =  new Pessoa[0];
	private Emprestimo[] emprestimos =  new Emprestimo[0];
	private Usuario[] usuarios =  new Usuario[0];
	private Cargo[] cargos =  new Cargo[0];
	private Permissao[] permissoes =  new Permissao[0];

	public Livro[] Livros { get => livros; set => livros = AddRange(value); }
	public Autor[] Autores { get => autores; set => autores = AddRange(value); }
	public Tag[] Tags { get => tags; set => tags = AddRange(value); }
	public Pessoa[] Pessoas { get => pessoas; set => pessoas = AddRange(value); }
	public Emprestimo[] Emprestimos { get => emprestimos; set => emprestimos = AddRange(value); }
	public Usuario[] Usuarios { get => usuarios; set => usuarios = AddRange(value); }
	public Cargo[] Cargos { get => cargos; set => cargos = AddRange(value); }
	public Permissao[] BDPermissoes { get => permissoes; set => permissoes = AddRange(value); }

	public void ResetarBanco()
	{
		BD.Database.EnsureDeleted();
		BD.Database.EnsureCreated();
	}
	public void Dispose()
	{
		BD.Dispose();
	}
}