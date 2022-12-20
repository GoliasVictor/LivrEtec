using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes;
public sealed class BDUtil : IDisposable
{
	
	public PacaContext CriarContexto(){
		return PacaContextFactory.CreateDbContext();
	}
	public PacaContextFactory PacaContextFactory { get; private set; }

	private ConfiguradorTestes _configuradorTestes;

	public BDUtil(ConfiguradorTestes configuradorTestes, Action<BDUtil> Configurar, Action<DbContextOptionsBuilder> configurarContext )
	{
		_configuradorTestes = configuradorTestes;	
		PacaContextFactory = new PacaContextFactory(_configuradorTestes.Config, configurarContext ,  _configuradorTestes.loggerFactory);
		Configurar(this);
		ResetarBanco();	
		using var BD =  CriarContexto();

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
