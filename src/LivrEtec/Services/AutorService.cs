using Microsoft.Extensions.Logging;

namespace LivrEtec;

public class AutorService : Repositorio
{
	public AutorService(IAcervoService acervoService) : base(acervoService)
	{
	}

	public bool Registrar(Autor autor)
	{
		BD.Autores.Add(autor);
		BD.SaveChanges();
		Logger?.LogInformation($"Autor @{autor.cd} registrado");
		return true;

	}
	public IEnumerable<Autor> Todos()
	{
		var Autores = BD.Autores.AsQueryable();
		Logger?.LogInformation("Todos Autores Coletados");
		return Autores;
	}
}