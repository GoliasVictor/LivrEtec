namespace LivrEtec;

public interface IRepAutores
{
	bool Registrar(Autor autor);
	IEnumerable<Autor> Todos();
}
