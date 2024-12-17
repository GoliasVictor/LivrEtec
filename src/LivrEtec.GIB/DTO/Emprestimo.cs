
using System.Diagnostics.CodeAnalysis;

namespace LivrEtec.GIB.DTO;

public record Emprestimo
(
    int Id,
    bool? AtrasoJustificado,
    string? Comentario,
    DateTime DataEmprestimo,
    DateTime? DataFechamento,
    bool? Devolvido,
    string? ExplicacaoAtraso,
    bool Fechado,
    DateTime FimDataEmprestimo,
    Livro Livro,
    Pessoa Pessoa,
    Usuario UsuarioCriador,
    Usuario? UsuarioFechador
){    
    [return: NotNullIfNotNull("model")]
    public static implicit operator Emprestimo?(LEM::Emprestimo? model)
        => model == null
         ? null : new (
             Id: model.Id,
             AtrasoJustificado: model.AtrasoJustificado,
             Comentario: model.Comentario,
             DataEmprestimo: model.DataEmprestimo,
             DataFechamento: model.DataFechamento,
             Devolvido: model.Devolvido,
             ExplicacaoAtraso: model.ExplicacaoAtraso,
             Fechado: model.Fechado,
             FimDataEmprestimo: model.FimDataEmprestimo,
             Livro: model.Livro,
             Pessoa: model.Pessoa,
             UsuarioCriador: model.UsuarioCriador,
             UsuarioFechador: model.UsuarioFechador
         );
}
