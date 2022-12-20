using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public sealed class RepAutores : Repositorio, IRepAutores
{
	public RepAutores(AcervoService acervoService) : base(acervoService)
	{
	}

	public async Task RegistrarAsync(Autor autor)
	{
		using var BD = await BDFactory.CreateDbContextAsync();
		BD.Autores.Add(autor);
		await BD.SaveChangesAsync();
		Logger?.LogInformation($"Autor @{autor.Id} registrado");
	}

	public IAsyncEnumerable<Autor> TodosAsync()
	{
		using var BD = BDFactory.CreateDbContext();
		var Autores = BD.Autores.AsAsyncEnumerable();
		Logger?.LogInformation("Todos Autores Coletados");
		return Autores;
	}
}