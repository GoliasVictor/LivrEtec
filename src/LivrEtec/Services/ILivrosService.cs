using LivrEtec.Models;

namespace LivrEtec.Services;

public interface ILivrosService
{
    Task<IEnumerable<Livro>> Buscar(string nome, string nomeAutor, IEnumerable<int>? idTags = null);
    Task Editar(Livro livro);
    Task<Livro?> Obter(int id);
    Task Registrar(Livro livro);
    Task Remover(int id);
}
