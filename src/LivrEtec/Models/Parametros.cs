namespace LivrEtec
{
	public record struct ParamBuscaEmprestimo (int? IdLivro, int? IdPessoa,bool? Fechado, bool? Atrasado){};
	public record struct ParamFecharEmprestimo (
        int IdEmprestimo,
		bool Devolvido,
		bool? AtrasoJustificado,
        string? ExplicacaoAtraso,
        int idUsuarioFechador 
	);
}