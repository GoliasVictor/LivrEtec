using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;

public sealed class RepAutores : Repositorio, IRepAutores
{
	public RepAutores(PacaContext BD, ILogger<RepAutores> logger) : base(BD, logger)
	{
	}

	public async Task RegistrarAsync(Autor autor)
	{
		Validador.ErroSeInvalido(autor);
		BD.Autores.Add(autor);
		await BD.SaveChangesAsync();
		logger?.LogInformation($"Autor @{autor.Id} registrado");
	}

	public IAsyncEnumerable<Autor> TodosAsync()
	{
		var Autores = BD.Autores.AsAsyncEnumerable();
		logger?.LogInformation("Todos Autores Coletados");
		return Autores;
	}
}