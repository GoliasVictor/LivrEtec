using System.Diagnostics.CodeAnalysis;

namespace LivrEtec.GIB.RPC;

public partial class Livro
{
    [return: NotNullIfNotNull("model")]
    public static implicit operator Livro?(LEM::Livro? model)
        => model == null
         ? null! : new()
         {
             Id = model.Id,
             Nome = model.Nome,
             Arquivado = model.Arquivado,
             Descricao = model.Descricao ?? "",
             Quantidade = model.Quantidade,
             Autores = { model.Autores.Select((modelAutor) => (Autor)modelAutor) },
             Tags = { model.Tags.Select((modelTag) => (Tag)modelTag) },
         };
    [return: NotNullIfNotNull("proto")]
    public static implicit operator LEM::Livro?(Livro? proto)
        => proto == null
         ? null! : new()
         {
             Id = proto.Id,
             Nome = proto.Nome,
             Arquivado = proto.Arquivado,
             Descricao = proto.Descricao,
             Quantidade = proto.Quantidade,
             Autores = proto.Autores.Select((a) => (LEM::Autor)a).ToList(),
             Tags = proto.Tags.Select((t) => (LEM::Tag)t).ToList()
         };
}
