using System.Diagnostics.CodeAnalysis;
using LE = LivrEtec;
namespace LivrEtec.GIB.RPC
{
    public partial class Tag
    {
        [return: NotNullIfNotNull("model")]
        public static implicit operator Tag?(LE::Tag? model)
            => model == null
             ? null : new()
            {
                Id = model.Id,
                Nome = model.Nome,
            };
        [return: NotNullIfNotNull("proto")]
        public static implicit operator LE::Tag?(Tag? proto)
            => proto == null
             ? null : new()
            {
                Id = proto.Id,
                Nome = proto.Nome,
            };
    }
}
