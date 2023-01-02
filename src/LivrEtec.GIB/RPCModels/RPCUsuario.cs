using System.Diagnostics.CodeAnalysis;
using LE = LivrEtec;

namespace LivrEtec.GIB.RPC
{
    public partial class Usuario
    {
        [return: NotNullIfNotNull("model")]
        public static implicit operator Usuario?( LE::Usuario? model) 
            => model == null
             ?  null : new(){
                Id = model.Id,
                Nome = model.Nome,
                Cargo = model.Cargo
            }; 
        [return: NotNullIfNotNull("proto")]
        public static implicit operator LE::Usuario?(Usuario? proto) 
            => proto == null
             ? null : new() {
                Id = proto.Id,
                Nome = proto.Nome,
                Cargo = proto.Cargo,
                Login = proto.Login,
                Senha = proto.Senha
            };
       
    }
}
