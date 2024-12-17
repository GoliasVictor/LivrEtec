
using System.Diagnostics.CodeAnalysis;

namespace LivrEtec.GIB.DTO;

public partial class Emprestimo
{
    int Id;
    bool? AtrasoJustificado;
    string? Comentario;
    DateTime DataEmprestimo;
    DateTime? DataFechamento;
    bool? Devolvido;
    string? ExplicacaoAtraso;
    bool Fechado;
    DateTime FimDataEmprestimo;
    Livro Livro;
    Pessoa Pessoa;
    Usuario UsuarioCriador;
    Usuario UsuarioFechador;
    [return: NotNullIfNotNull("model")]
    public static implicit operator Emprestimo?(LEM::Emprestimo? model)
        => model == null
         ? null : new()
         {
             Id = model.Id,
             AtrasoJustificado = model.AtrasoJustificado,
             Comentario = model.Comentario,
             DataEmprestimo = model.DataEmprestimo,
             DataFechamento = model.DataFechamento,
             Devolvido = model.Devolvido,
             ExplicacaoAtraso = model.ExplicacaoAtraso,
             Fechado = model.Fechado,
             FimDataEmprestimo = model.FimDataEmprestimo,
             Livro = model.Livro,
             Pessoa = model.Pessoa,
             UsuarioCriador = model.UsuarioCriador,
             UsuarioFechador = model.UsuarioFechador
         };
    [return: NotNullIfNotNull("proto")]
    public static implicit operator LEM::Emprestimo?(Emprestimo? proto)
        => proto == null
         ? null : new()
         {
             Id = proto.Id,
             AtrasoJustificado = proto.AtrasoJustificado,
             Comentario = proto.Comentario,
             DataEmprestimo = proto.DataEmprestimo,
             DataFechamento = proto.DataFechamento,
             Devolvido = proto.Devolvido,
             ExplicacaoAtraso = proto.ExplicacaoAtraso,
             Fechado = proto.Fechado,
             FimDataEmprestimo = proto.FimDataEmprestimo,
             Livro = proto.Livro,
             Pessoa = proto.Pessoa,
             UsuarioCriador = proto.UsuarioCriador,
             UsuarioFechador = proto.UsuarioFechador

         };
}
