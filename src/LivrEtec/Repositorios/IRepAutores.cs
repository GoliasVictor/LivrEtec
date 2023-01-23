using LivrEtec.Models;

namespace LivrEtec.Repositorios;

public interface IRepAutores
{
	Task Registrar(Autor autor);
	IAsyncEnumerable<Autor> Todos();
}
