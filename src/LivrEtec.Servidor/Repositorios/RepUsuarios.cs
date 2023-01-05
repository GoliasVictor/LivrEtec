namespace LivrEtec.Servidor
{
	public class RepUsuarios : Repositorio, IRepUsuarios
	{

		public RepUsuarios(AcervoService acervoService) : base(acervoService)
		{
		}

		public async Task<Usuario?> ObterAsync(int id)
		{
			
			var usuario = await BD.Usuarios.FindAsync(id);
			if (usuario == null)
				return null;
			await BD.Entry(usuario).Reference(u=> u.Cargo).LoadAsync();
			await BD.Entry(usuario.Cargo).Collection(c=> c.Permissoes).LoadAsync();
			return usuario;
		}
		public async Task<bool> ExisteAsync(int id)
		{
			return await Task.Run<bool>(()=> BD.Usuarios.Any(u => u.Id == id));
		}
 

	}
}