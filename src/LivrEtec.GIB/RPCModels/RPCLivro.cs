using System.Diagnostics.CodeAnalysis;


namespace LivrEtec.GIB.RPC;
public class Livro
{

    public int Id { get; set; }
    public String Nome { get; set; }
    public bool Arquivado { get; set; }
    public string Descricao { get; set; }
    public int Quantidade { get; set; }
    public List<DTOAutor> Autores { get; set; }
    public List<Tag> Tags { get; set; }

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
             Autores = model.Autores.Select((modelAutor) => (RPC::DTOAutor)modelAutor).ToList(),
             Tags = model.Tags.Select((modelTag) => (RPC::Tag)modelTag).ToList(),
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
