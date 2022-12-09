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
                Autores = proto.Autores.Cast<LE::Autor>().ToList(),
                Tags = proto.Tags.Cast<LE::Tag>().ToList() 
            };
    }
}
