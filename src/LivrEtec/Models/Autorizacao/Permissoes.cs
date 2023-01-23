using LivrEtec.Models.Autorizacao;

namespace LivrEtec;
public static partial class Permissoes
{

    public static readonly CRUD Livro;
    public static readonly CRUD Tag;
    public static readonly CRUD Autor;
    public static readonly CRUD Cargo;
    public static readonly CRUD Pessoa;
    public static readonly CRUD Usuario;
    public static readonly PermissoesEmprestimo Emprestimo;
    public static IGrupoPermissao[] TodosGrupos
        => new[] { Livro, Tag, Autor, Usuario, Cargo, Pessoa, Emprestimo };
    public static Permissao[] TodasPermissoes
        => TodosGrupos.SelectMany(grupo => grupo.Todas).ToArray();

    static Permissoes()
    {
        var UltimoID = 1;
        Livro = new(nameof(Livro), UltimoID);
        Tag = new(nameof(Tag), UltimoID += 4);
        Autor = new(nameof(Autor), UltimoID += 4);
        Usuario = new(nameof(Usuario), UltimoID += 4);
        Cargo = new(nameof(Cargo), UltimoID += 4);
        Pessoa = new(nameof(Pessoa), UltimoID += 4);
        Emprestimo = new(nameof(Emprestimo), UltimoID += 4, Livro.Visualizar, Pessoa.Visualizar);

    }

}