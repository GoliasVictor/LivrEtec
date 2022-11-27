using LivrEtec;
using LivrEtec.Testes;
using Microsoft.Extensions.Logging;





var Cargos = new[]{
			new Cargo(1, "Administrador", Permissoes.TodasPermissoes.ToList()),
			new Cargo(2, "Aluno", new (){
				Permissoes.Livro.Criar,
				Permissoes.Livro.Visualizar,
			}),
			new Cargo(3, "Aluno Estagiario", new(){
				Permissoes.Livro.Criar,
				Permissoes.Livro.Visualizar,
				Permissoes.Livro.Editar,
				Permissoes.Livro.Excluir,
				Permissoes.Emprestimo.Criar,
				Permissoes.Emprestimo.Excluir,
			})
		};
Cargo gCargo(int id) => Cargos.First((c)=> c.Id == id); 
Usuario[] Usuarios = new[]{
			new Usuario(1, "Senha", "tavares", "Tavares", gCargo(1)),
			new Usuario(2, "Senha", "Ze", "Zé", gCargo(2)),
			new Usuario(3, "Senha", "Atlas", "Atlas", gCargo(3)),
		};
Usuario gUsuario(int id) => Usuarios.First((u)=> u.Id == id); 
var BD = new PacaContext(
	new ConfiguracaoTeste{ StrConexaoMySQL = "server=localhost;database=LivrEtecBD;user=LivrEtecServe;password=LivrEtecSenha"},
	LoggerFactory.Create((lb)=> { 
		lb.AddConsole();
		lb.AddFilter((_,_, logLevel)=> logLevel >= LogLevel.Information);
})
);

BD.Database.EnsureDeleted();
BD.Database.EnsureCreated();
BD.Permissoes.AddRange(Permissoes.TodasPermissoes);
BD.Cargos.Add(Cargos[0]);
BD.SaveChanges();
BD.Cargos.Add(Cargos[1]);

BD.Cargos.Add(Cargos[2]);
BD.SaveChanges();
BD.Usuarios.AddRange(Usuarios);
BD.SaveChanges();


BD.SaveChanges();
var AutorizacaoService = new AutorizacaoService(BD, null!);

var usuario = gUsuario(1);
var permissao =  Permissoes.Cargo.Criar;

var autorizado = AutorizacaoService.EhAutorizado(usuario, permissao);

Assert.False(autorizado);