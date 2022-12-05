using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public sealed class RepAutores : Repositorio, IRepAutores
{
	public RepAutores(AcervoService acervoService) : base(acervoService)
	{
	}

	public bool Registrar(Autor autor)
	{
		BD.Autores.Add(autor);
		BD.SaveChanges();
		Logger?.LogInformation($"Autor @{autor.Id} registrado");
		return true;

	}
	public IEnumerable<Autor> Todos()
	{
		var Autores = BD.Autores.AsQueryable();
		Logger?.LogInformation("Todos Autores Coletados");
		return Autores;
	}
}