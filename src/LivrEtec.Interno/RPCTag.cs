namespace LivrEtec.Interno.RPC
{
    public partial class RPCTag
    {
        public static implicit operator RPCTag(Tag model)
            => new()
            {
                Id = model.Id,
                Nome = model.Nome,
            };
        public static implicit operator Tag(RPCTag proto)
            => new()
            {
                Id = proto.Id,
                Nome = proto.Nome,
            };
    }
}
