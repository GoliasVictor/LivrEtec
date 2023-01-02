namespace LivrEtec.Servidor
{
	internal class RepUsuarios : Repositorio, IRepUsuarios
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
			return usuario;
		}
 

	}
}