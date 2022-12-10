using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public sealed class RepAutores : Repositorio, IRepAutores
{
	public RepAutores(AcervoService acervoService) : base(acervoService)
	{
	}

	public void Registrar(Autor autor)
	{
		BD.Autores.Add(autor);
		BD.SaveChanges();
		Logger?.LogInformation($"Autor @{autor.Id} registrado");
	}
	public async Task RegistrarAsync(Autor autor)
	{
		BD.Autores.Add(autor);
		await BD.SaveChangesAsync();
		Logger?.LogInformation($"Autor @{autor.Id} registrado");
	}
	public IEnumerable<Autor> Todos()
	{
		var Autores = BD.Autores.AsQueryable();
		Logger?.LogInformation("Todos Autores Coletados");
		return Autores;
	}

	IAsyncEnumerable<Autor> IRepAutores.TodosAsync()
	{
		var Autores = BD.Autores.AsAsyncEnumerable();
		Logger?.LogInformation("Todos Autores Coletados");
		return Autores;
	}
}