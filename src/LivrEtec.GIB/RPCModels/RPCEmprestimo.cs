using System.Diagnostics.CodeAnalysis;
using Google.Protobuf.WellKnownTypes;

namespace LivrEtec.GIB.RPC
{
    public partial class Emprestimo
    {
        [return: NotNullIfNotNull("model")]
        public static implicit operator Emprestimo?(LEM::Emprestimo? model)
            => model == null
             ? null : new()
            {
                Id = model.Id,
                AtrasoJustificado = model.AtrasoJustificado,
                Comentario =  model.Comentario,
                DataEmprestimo =  Timestamp.FromDateTime(model.DataEmprestimo),
                DataFechamento =  model.DataFechamento != null ? Timestamp.FromDateTime( model.DataFechamento.Value) : null,
                Devolvido = model.Devolvido,
                ExplicacaoAtraso = model.ExplicacaoAtraso,
                Fechado = model.Fechado,
                FimDataEmprestimo = Timestamp.FromDateTime(model.FimDataEmprestimo),
                Livro = model.Livro,
                Pessoa = model.Pessoa,
                UsuarioCriador = model.UsuarioCriador,
#pragma warning disable CS8604 // Não importar ser nulo
                UsuarioFechador = model.UsuarioFechador
#pragma warning restore CS8604 
                
            };
        [return: NotNullIfNotNull("proto")]
        public static implicit operator LEM::Emprestimo?(Emprestimo? proto)
            => proto == null
             ? null : new()
            {
                Id = proto.Id,
                AtrasoJustificado = proto.AtrasoJustificado,
                Comentario =  proto.Comentario,
                DataEmprestimo =  proto.DataEmprestimo.ToDateTime(),
                DataFechamento = proto.DataFechamento.ToDateTime(),
                Devolvido = proto.Devolvido,
                ExplicacaoAtraso = proto.ExplicacaoAtraso,
                Fechado = proto.Fechado,
                FimDataEmprestimo = proto.FimDataEmprestimo.ToDateTime(),
                Livro = proto.Livro,
                Pessoa = proto.Pessoa,
                UsuarioCriador = proto.UsuarioCriador,
                UsuarioFechador = proto.UsuarioFechador

            };
    }
}
