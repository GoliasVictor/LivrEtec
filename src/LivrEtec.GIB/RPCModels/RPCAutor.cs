using System.Diagnostics.CodeAnalysis;

namespace LivrEtec.GIB.RPC;

public class DTOAutor
{
    public int Id { get; set; }
    public string Nome { get; set; }
    [return: NotNullIfNotNull("model")]
    public static implicit operator DTOAutor(LEM::Autor model)
        => model == null
         ? null! : new()
         {
             Id = model.Id,
             Nome = model.Nome,
         };
    [return: NotNullIfNotNull("proto")]
    public static implicit operator LEM::Autor(DTOAutor proto)
        => proto == null
         ? null! : new()
         {
             Id = proto.Id,
             Nome = proto.Nome,
         };

}
