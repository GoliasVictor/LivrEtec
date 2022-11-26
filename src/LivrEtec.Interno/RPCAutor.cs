

namespace LivrEtec.Interno.RPC
{
    public partial class RPCAutor
    {
        public static implicit operator RPCAutor?(Autor? model) 
            => model == null
             ?  null : new(){
                Id = model.Id,
                Nome = model.Nome,
            }; 
        public static implicit operator Autor?(RPCAutor? proto) 
            => proto == null
             ? null : new() {
                Id = proto.Id,
                Nome = proto.Nome,
            };
       
    }
}
