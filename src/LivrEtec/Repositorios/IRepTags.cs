namespace LivrEtec.Servidor
{
    public interface IRepTags
    {
		Task<int> Registrar(Tag tag);
		Task Editar(Tag tag);
		Task<IEnumerable<Tag>> Buscar(string nome);
		Task<Tag?> Obter(int id);
		Task Remover(int id);
    }
}
