namespace LivrEtec.Repositorios;
/// <summary>
/// Repositorio que define métodos para acessar e editar dados de autores de livros
/// </summary>
/// <remarks>
/// Define apenas metodos assincronos 
/// </remarks>
/// <remark>
/// Não é garantido que os objetos ao serem passados como parametros continuem o mesmo posteriormente.
/// </remark>
public interface IRepAutores
{
	/// <summary>
    /// Registra um autor
    /// </summary>
    /// <param name="autor">
    /// autor a ser registrado
    /// </param> 
    /// <exception cref="ValidationException">Objeto invalido</exception>
	Task Registrar(Autor autor);
	/// <summary>
    /// Busca por todos os autores 
    /// </summary>
    /// <returns>Todos os autores encontrados</returns>
	IAsyncEnumerable<Autor> Todos();
}
