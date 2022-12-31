using System.Diagnostics.CodeAnalysis;
using LE = LivrEtec;

namespace LivrEtec.GIB.RPC
{
    public partial class Permissao
    {
        [return: NotNullIfNotNull("model")]
        public static implicit operator Permissao?(LE::Permissao? model) 
            => model == null
             ?  null : new(){
                Id = model.Id,
                Nome = model.Nome,
                PermissoesDependete = { model.PermissoesDependete.Select( p => (Permissao)p)  },
            }; 
        [return: NotNullIfNotNull("proto")]
        public static implicit operator LE::Permissao?(Permissao? proto) 
            => proto == null
             ? null : new() {
                Id = proto.Id,
                Nome = proto.Nome,
                Descricao = proto.Descricao,
                PermissoesDependete = proto.PermissoesDependete.Select( p => (LE::Permissao)p) .ToList()
            };
       
    }
}
