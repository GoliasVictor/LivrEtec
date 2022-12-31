using Grpc.Core;

namespace LivrEtec.GIB.Servidor;
// Gambiarra pra fazer funcionar por enquanto
internal class IdentidadeServiceRPC : IIdentidadeService
{
	ServerCallContext? context;
	public IdentidadeServiceRPC()
	{
	}
	public void DefinirContexto(ServerCallContext context){
		this.context = context;
	}
	public int IdUsuario => context != null ? int.Parse(context.GetHttpContext().Request.Headers["id"][0])
											: throw new InvalidOperationException("contexto indefinido");
	public Usuario? Usuario => new Usuario{ Id = IdUsuario };
	public bool EstaAutenticado => true;
	public Task AutenticarUsuarioAsync(string senha) => Task.CompletedTask; 
	public Task DefinirUsuarioAsync(int idUsuario)  => Task.CompletedTask;
	public Task<bool> EhAutorizadoAsync(Permissao permissao) => Task.FromResult(true);
	public Task ErroSeNaoAutorizadoAsync(Permissao permissao) => Task.CompletedTask;
}