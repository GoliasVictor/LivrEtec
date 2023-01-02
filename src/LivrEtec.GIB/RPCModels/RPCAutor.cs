using System.Diagnostics.CodeAnalysis;
using LE = LivrEtec;

namespace LivrEtec.GIB.RPC
{
    public partial class Autor
    {
        [return: NotNullIfNotNull("model")]
        public static implicit operator Autor(LE::Autor model) 
            => model == null
             ?  null! : new(){
                Id = model.Id,
                Nome = model.Nome,
            }; 
        [return: NotNullIfNotNull("proto")]
        public static implicit operator LE::Autor(Autor proto) 
            => proto == null
             ? null! : new() {
                Id = proto.Id,
                Nome = proto.Nome,
            };
       
    }
}
