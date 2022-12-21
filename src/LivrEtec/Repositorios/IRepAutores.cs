namespace LivrEtec;

public interface IRepAutores
{
	Task RegistrarAsync(Autor autor);
	IAsyncEnumerable<Autor> TodosAsync();
}
