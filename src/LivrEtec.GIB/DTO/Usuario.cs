using System.Diagnostics.CodeAnalysis;


namespace LivrEtec.GIB.DTO;

public record Usuario (
    int Id,
    string Nome,
    Cargo Cargo
){
    [return: NotNullIfNotNull("model")]
    public static implicit operator Usuario?(LEM::Usuario? model)
        => model == null
         ? null : new(
             Id: model.Id,
             Nome: model.Nome,
             Cargo: model.Cargo
         );
}
