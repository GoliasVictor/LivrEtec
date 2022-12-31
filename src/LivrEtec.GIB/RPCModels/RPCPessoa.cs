using System.Diagnostics.CodeAnalysis;
using LE = LivrEtec;

namespace LivrEtec.GIB.RPC
{
    public partial class Pessoa
    {
        [return: NotNullIfNotNull("model")]
        public static implicit operator Pessoa?( LE::Pessoa? model) 
            => model == null
             ?  null : new(){
                Id = model.Id,
                Nome = model.Nome,
                Telefone =  model.Telefone
            }; 
        [return: NotNullIfNotNull("proto")]
        public static implicit operator LE::Pessoa?(Pessoa? proto) 
            => proto == null
             ? null : new() {
                Id = proto.Id,
                Nome = proto.Nome,
                Telefone = proto.Telefone
            };
       
    }
}
