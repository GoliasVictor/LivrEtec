using System.Diagnostics.CodeAnalysis;


namespace LivrEtec.GIB.DTO;
public record  Tag (int Id,string Nome)
{

    [return: NotNullIfNotNull(nameof(model))]
    public static implicit operator Tag?(LEM::Tag? model)
        => model == null
         ? null : new(
            Id: model.Id, 
            Nome: model.Nome
        );
    [return: NotNullIfNotNull(nameof(proto))]
    public static implicit operator LEM::Tag?(Tag? proto)
        => proto == null
         ? null : new()
         {
             Id = proto.Id,
             Nome = proto.Nome,
         };
}
