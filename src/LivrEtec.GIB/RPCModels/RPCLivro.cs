using LE = LivrEtec;
namespace LivrEtec.GIB.RPC
{
    public partial class Livro
    {
        public static implicit operator Livro(LE::Livro model)
            => model == null
             ? null! : new()
            {
                Id = model.Id,
                Nome = model.Nome,
                Arquivado =  model.Arquivado,
                Descricao = model.Descricao,
                Quantidade = model.Quantidade,
                Autores  = {model.Autores.Select((modelAutor)=> (Autor)modelAutor) },
                Tags  = {model.Tags.Select( (modelTag)=> (Tag)modelTag )}
            };
        public static implicit operator LE::Livro?(Livro? proto)
            => proto == null
             ? null! : new()
            {
                Id = proto.Id,
                Nome = proto.Nome,
                Arquivado = proto.Arquivado,
                Descricao = proto.Descricao,
                Quantidade = proto.Quantidade,
                Autores = proto.Autores.Select((a)=> (LE::Autor)a).ToList(),
                Tags = proto.Tags.Select((t)=> (LE::Tag)t).ToList() 
            };
    }
}
