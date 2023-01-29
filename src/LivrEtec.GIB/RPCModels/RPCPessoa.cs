using System.Diagnostics.CodeAnalysis;


namespace LivrEtec.GIB.RPC;

public partial class Pessoa
{
    [return: NotNullIfNotNull("model")]
    public static implicit operator Pessoa?(LEM::Pessoa? model)
        => model == null
         ? null : new()
         {
             Id = model.Id,
             Nome = model.Nome,
             Telefone = model.Telefone
         };
    [return: NotNullIfNotNull("proto")]
    public static implicit operator LEM::Pessoa?(Pessoa? proto)
        => proto == null
         ? null : new()
         {
             Id = proto.Id,
             Nome = proto.Nome,
             Telefone = proto.Telefone
         };

}
