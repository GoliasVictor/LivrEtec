namespace LivrEtec;

public interface IRepAutores
{
	void Registrar(Autor autor);
	Task RegistrarAsync(Autor autor);
	IEnumerable<Autor> Todos();
	IAsyncEnumerable<Autor> TodosAsync();
}
