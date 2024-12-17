using System.Diagnostics.CodeAnalysis;

namespace LivrEtec.GIB.DTO;

public record Autor
(
    int Id,
    string Nome
){
    [return: NotNullIfNotNull("model")]
    public static implicit operator Autor(LEM::Autor model)
        => model == null
         ? null! : new(
             Id: model.Id,
             Nome: model.Nome
         );
    [return: NotNullIfNotNull("proto")]
    public static implicit operator LEM::Autor(Autor proto)
        => proto == null
         ? null! : new()
         {
             Id = proto.Id,
             Nome = proto.Nome,
         };

}
