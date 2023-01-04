using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LivrEtec.Testes;
public class BDUtil : IDisposable
{
	
	public PacaContext CriarContexto(){
		return PacaContextFactory.CreateDbContext();
	}
	public PacaContextFactory PacaContextFactory { get; private set; }

	public void SalvarDados()
    {
        ResetarBanco();
        using var BD = CriarContexto();

        BD.Permissoes.AddRange(BDPermissoes);
        BD.Cargos.AddRange(Cargos);
        BD.Usuarios.AddRange(Usuarios);

        BD.Tags.AddRange(Tags);
        BD.Autores.AddRange(Autores);
        BD.Livros.AddRange(Livros);

        BD.Pessoas.AddRange(Pessoas);
        BD.Emprestimos.AddRange(Emprestimos);

        BD.SaveChanges();
        BD.ChangeTracker.Clear();

    }
	
    protected BDUtil(DbContextOptions<PacaContext> contextOptions )
	{

        PacaContextFactory = new PacaContextFactory(contextOptions);
    }

    public Livro[] Livros { get; set; } =  new Livro[0];
	public Autor[] Autores { get; set; } =  new Autor[0];
	public Tag[] Tags { get; set; } =  new Tag[0];
	public Pessoa[] Pessoas { get; set; } =  new Pessoa[0];
	public Emprestimo[] Emprestimos { get; set; } =  new Emprestimo[0];
	public Usuario[] Usuarios { get; set; } =  new Usuario[0];
	public Cargo[] Cargos { get; set; } =  new Cargo[0];
	public Permissao[] BDPermissoes { get; set; } =  new Permissao[0];
	
	public Autor gAutor(int id) => Autores.First((a)=> a.Id == id); 
	public Tag gTag(int id) => Tags.First((a)=> a.Id == id); 
	public Livro gLivro(int id) => Livros.First((l)=> l.Id == id);
	public Usuario gUsuario(int id) => Usuarios.First((u) => u.Id == id);
	public Cargo gCargo(int id) => Cargos.First((c)=> c.Id == id);  
	public Pessoa gPessoa(int id) => Pessoas.First((c)=> c.Id == id);  
	public Emprestimo gEmprestimo(int id) => Emprestimos.First((e)=> e.Id == id);  
	public async Task<Emprestimo?> gEmprestimoBanco(int idEmprestimo)
	{
		using var BD = CriarContexto();
		var emprestimoAtual = await BD.Emprestimos.FindAsync(idEmprestimo);
		if(emprestimoAtual is not null){

			BD.Entry(emprestimoAtual).Reference((e) => e.Pessoa).Load();
			BD.Entry(emprestimoAtual).Reference((e) => e.UsuarioCriador).Load();
			BD.Entry(emprestimoAtual).Reference((e) => e.UsuarioFechador).Load();
			BD.Entry(emprestimoAtual).Reference((e) => e.Livro).Load();
		}
		return emprestimoAtual;
	}
	public void ResetarBanco()
	{
		using var BD = CriarContexto();
		BD.Database.EnsureDeleted();
		BD.Database.EnsureCreated();  
	}
	public void Dispose()
	{
	}
}

public sealed class BDUtilMySQl : BDUtil {
	public BDUtilMySQl(string strConexaoMySQL, ILoggerFactory loggerFactory) 
		: base(new DbContextOptionsBuilder<PacaContext>()
				.UseLoggerFactory(loggerFactory)
				.UseMySql(strConexaoMySQL, ServerVersion.AutoDetect(strConexaoMySQL))
				.Options
		){}
}
public sealed class BDUtilSqlLite : BDUtil {
	public BDUtilSqlLite(ILoggerFactory loggerFactory) 
		: base( new DbContextOptionsBuilder<PacaContext>()
				.UseLoggerFactory(loggerFactory)
				.UseSqlite($"DataSource=Contatos.db")
				.Options
		){}
}