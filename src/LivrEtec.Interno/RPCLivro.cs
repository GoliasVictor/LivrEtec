namespace LivrEtec.Interno.RPC
{
    public partial class RPCLivro
    {
        public static implicit operator RPCLivro?(Livro? model)
            => model == null
             ? null : new()
            {
                Id = model.Id,
                Nome = model.Nome,
                Arquivado =  model.Arquivado,
                Autores  = {model.Autores.Select((modelAutor)=> (RPCAutor)modelAutor) },
                Tags  = {model.Tags.Select( (modelTag)=> (RPCTag)modelTag )}
            };
        public static implicit operator Livro?(RPCLivro? proto)
            => proto == null
             ? null : new()
            {
                Id = proto.Id,
                Nome = proto.Nome,
                Arquivado = proto.Arquivado,
                Autores = proto.Autores.Cast<Autor>().ToList(),
                Tags = proto.Tags.Cast<Tag>().ToList() 
            };
    }
}
