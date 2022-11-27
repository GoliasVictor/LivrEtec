namespace LivrEtec;

public interface IAutorService
{
	bool Registrar(Autor autor);
	IEnumerable<Autor> Todos();
}
