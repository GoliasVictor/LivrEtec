using System.Diagnostics.CodeAnalysis;

namespace LivrEtec.GIB.RPC;

public class Cargo
{
    int Id;
    string Nome;
    List<Permissao> Permissoes; 
    [return: NotNullIfNotNull("model")]
    public static implicit operator Cargo?(LEM::Cargo? model)
        => model == null
         ? null : new()
         {
             Id = model.Id,
             Nome = model.Nome,
             Permissoes = model.Permissoes.Select(p => (Permissao)p).ToList(),
         };
    [return: NotNullIfNotNull("proto")]
    public static implicit operator LEM::Cargo?(Cargo? proto)
        => proto == null
         ? null : new()
         {
             Id = proto.Id,
             Nome = proto.Nome,
             Permissoes = proto.Permissoes.Select(p => (LEM::Permissao)p).ToList()
         };

}
