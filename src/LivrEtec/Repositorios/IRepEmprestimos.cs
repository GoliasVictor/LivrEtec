using LivrEtec.Exceptions;

namespace LivrEtec.Repositorios;

/// <summary>
/// Repositorio que define métodos para persistência de dados de emprestimos
/// </summary>
public interface IRepEmprestimos
{
    /// <summary>
    /// Calcula a quantidade de exemplares do livro estão sendo emprestados atualmente
    /// </summary>
    /// <param name="idLivro">O id dos livro</param>
    /// <returns>A quantidade de exemplares empretados</returns>
    Task<int> ObterQuantidadeLivrosEmprestado(int idLivro);
    /// <summary>
    /// Registra um emprestimo
    /// </summary>
    /// <param name="emprestimo">O emprestimo a ser registrado</param>
    /// <returns>O id do emprestimo</returns>
    /// <exception cref="ValidationException">Emprestimo invalido</exception>
    Task<int> Registrar(Emprestimo emprestimo);
    /// <summary>
    /// Busca um emprestimo
    /// </summary>
    /// <param name="idEmprestimo">O id do emprestimo a ser buscado</param>
    /// <returns>O  emprestimo caso o encontre, caso contrário retorna <see langword="null"/></returns>
    Task<Emprestimo?> Obter(int idEmprestimo);

    /// <summary>
    /// Busca todos os usuarios que satifazem a seguintes condições todas as seguintes condições
    /// <list type="bullet">
    /// <item> Caso <c>parametros.IdLivro</c> não seja <see langword="null"/> apenas os emprestimos do livro indicado por <c>parametros.IdLivro</c></item>
    /// <item> Caso <c>parametros.IdPessoa</c> não seja <see langword="null"/> retornara emprestimos que foram emprestados pela pessoa indicada por <c>parametros.IdPessoa</c> </item>
    /// <item> Caso <c>parametros.Fechado</c> seja <see langword="true"/> retornara apenas os emprestimos fechados  </item>
    /// <item> Caso <c>parametros.Fechado</c> seja <see langword="false"/> retornara apenas os emprestimos abertos  </item>
    /// <item> Caso <c>parametros.Atrasado</c> seja <see langword="true"/>  retornara apenas os emprestimos atrasados  </item>
    /// <item> Caso <c>parametros.Atrasado</c> seja <see langword="false"/> retornara apenas os emprestimos que não estão estão atrasados</item>
    ///</list>
    /// </summary>
    /// <param name="parametros"></param>
    /// <returns>Emprestimos apos todos </returns>
    /// <example>
    /// Esse codigo busca todos os emprestimos abertos que estão atrasados
    /// <code>
    /// await emprestimoService.Buscar(new ParamBuscaEmprestimo(
    ///     IdLivro: null,
    ///     IdPessoa: null,
    ///     Fechado: false,
    ///     Atrasado: true
    /// ))
    /// </code>
    /// Já este busca todos os emprestimos já fechados do livro de id 5
    /// <code>
    /// await emprestimoService.Buscar(new ParamBuscaEmprestimo(
    ///     IdLivro: 5,
    ///     IdPessoa: null,
    ///     Fechado: true,
    ///     Atrasado: null
    /// ))
    /// </code>
    /// </example>
    Task<IEnumerable<Emprestimo>> Buscar(ParamBuscaEmprestimo parametros);
    /// <summary>
    /// Define o emprestimo como fechado
    /// </summary>
    /// <param name="parametros"> <seealso cref="ParamFecharEmprestimo"/></param>
    /// <exception cref="InvalidOperationException">O id do usuário ou o emprestimo não estão registrados</exception>
    Task Fechar(ParamFecharEmprestimo parametros);
    
    Task EditarFimData(int idLivro, DateTime NovaData);
    Task Excluir(int idEmprestimo);
}