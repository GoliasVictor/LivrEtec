using LivrEtec.Models;

namespace LivrEtec.Services;

public interface ITagsService
{
    Task<int> Registrar(Tag tag);
    Task Editar(Tag tag);
    Task<IEnumerable<Tag>> Buscar(string nome);
    Task<Tag?> Obter(int id);
    Task Remover(int id);
}
