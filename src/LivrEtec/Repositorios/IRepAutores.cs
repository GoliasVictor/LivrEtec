namespace LivrEtec;

public interface IRepAutores
{
	Task Registrar(Autor autor);
	IAsyncEnumerable<Autor> Todos();
}
