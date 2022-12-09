using LE = LivrEtec;

namespace LivrEtec.GIB.RPC
{
    public partial class Autor
    {
        public static implicit operator Autor(LE::Autor model) 
            => model == null
             ?  null! : new(){
                Id = model.Id,
                Nome = model.Nome,
            }; 
        public static implicit operator LE::Autor(Autor proto) 
            => proto == null
             ? null! : new() {
                Id = proto.Id,
                Nome = proto.Nome,
            };
       
    }
}
