namespace LivrEtec.Services;
/// <summary>
/// Serviço dedicado a veririficar se usuario é autorizado a tomar alguma ação
/// </summary> 
public interface IAutorizacaoService
{
    /// <summary>
    /// Verifica se o <paramref name="usuario"/> possue e a  permissão indicada por <paramref name="permissao"/>
    /// </summary>
    /// <remark> 
    /// </remark>
    /// <param name="usuario">Usuario a ser verificada a permissão</param>
    /// <param name="permissao"> Permissão a ser vericada </param>
    /// <returns><see langword="false"/> caso o o usuario não possua a permissão ou o usuario seja nulo, <see langword="true"/> caso o usuario possua a permissão  </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="permissao"/> é  <see langword="null"/></exception>
    /// <exception cref="ArgumentException"> A permissão ou o usuario não existe</exception>
    Task<bool> EhAutorizado(Usuario usuario, Permissao permissao);


    /// <summary>
    /// Verifica se o usuario com o id iguala <paramref name="idUsuario"/> possue e a permissão indicada por <paramref name="permissao"/>
    /// </summary>
    /// <param name="idUsuario">Id do Usuario a ser verificada a permissão</param>
    /// <param name="permissao"> Permissão a ser vericada </param>
    /// <returns><c>false</c> caso o o usuario não possua a permissão ou o usuario seja nulo, <c>true</c> caso o usuario possua a permissão  </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="permissao"/> é  <c>null</c></exception>
    /// <exception cref="ArgumentException"> A permissão ou o usuario não existe</exception>
    Task<bool> EhAutorizado(int idUsuario, Permissao permissao);
    Task ErroSeNaoAutorizado(Usuario usuario, Permissao permissao);
}
