using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor
{
	public class RepUsuarios : Repositorio, IRepUsuarios
	{

		public RepUsuarios(PacaContext BD, ILogger<RepUsuarios> logger) : base(BD, logger)
		{
		}

		public async Task<Usuario?> Obter(int id)
		{
			
			var usuario = await BD.Usuarios.FindAsync(id);
			if (usuario == null)
				return null;
			await BD.Entry(usuario).Reference(u=> u.Cargo).LoadAsync();
			await BD.Entry(usuario.Cargo).Collection(c=> c.Permissoes).LoadAsync();
			return usuario;
		}
		public async Task<bool> Existe(int id)
		{
			return await Task.Run<bool>(()=> BD.Usuarios.Any(u => u.Id == id));
		}
 

	}
}