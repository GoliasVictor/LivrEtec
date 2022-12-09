using LE = LivrEtec;
namespace LivrEtec.GIB.RPC
{
    public partial class Tag
    {
        public static implicit operator Tag(LE.Tag model)
            => new()
            {
                Id = model.Id,
                Nome = model.Nome,
            };
        public static implicit operator LE.Tag(Tag proto)
            => new()
            {
                Id = proto.Id,
                Nome = proto.Nome,
            };
    }
}
