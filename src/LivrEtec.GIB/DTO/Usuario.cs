using System.Diagnostics.CodeAnalysis;


namespace LivrEtec.GIB.DTO;

public partial class Usuario
{
    int Id;
    string Nome;
    Cargo Cargo;
    string Login;
    string Senha; 
    [return: NotNullIfNotNull("model")]
    public static implicit operator Usuario?(LEM::Usuario? model)
        => model == null
         ? null : new()
         {
             Id = model.Id,
             Nome = model.Nome,
             Cargo = model.Cargo
         };
    [return: NotNullIfNotNull("proto")]
    public static implicit operator LEM::Usuario?(Usuario? proto)
        => proto == null
         ? null : new()
         {
             Id = proto.Id,
             Nome = proto.Nome,
             Cargo = proto.Cargo,
             Login = proto.Login,
             Senha = proto.Senha
         };

}
