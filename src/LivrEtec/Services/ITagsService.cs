namespace LivrEtec;

public interface ITagsService
{
	Task<int> RegistrarAsync(Tag tag);
	Task EditarAsync(Tag tag);
	Task<IEnumerable<Tag>> BuscarAsync(string nome);
	Task<Tag?> ObterAsync(int id);
	Task RemoverAsync(int id);
}
