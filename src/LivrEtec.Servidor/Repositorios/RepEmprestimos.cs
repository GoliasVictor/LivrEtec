using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec.Servidor
{
    public class RepEmprestimos : Repositorio, IRepEmprestimos
    {
        IRelogio _relogio ;
        IRepUsuarios repUsuarios;
        public RepEmprestimos(PacaContext BD, IRepUsuarios repUsuarios, ILogger<RepEmprestimos> logger, IRelogio relogio) : base(BD, logger)
        {
            this.repUsuarios = repUsuarios;
            _relogio = relogio;
        }
        public async Task<int> Registrar(Emprestimo emprestimo)
        {
		    Validador.ErroSeInvalido(emprestimo);
            BD.Attach(emprestimo);
            BD.Entry(emprestimo).State = EntityState.Added;
            await BD.SaveChangesAsync();
            return emprestimo.Id;

        }
        public async Task<int> ObterQuantidadeLivrosEmprestado(int idLivro)
        {
            return await BD.Emprestimos.CountAsync((e) => e.Livro.Id == idLivro && !e.Fechado);
        }

		public async Task<IEnumerable<Emprestimo>> Buscar(ParamBuscaEmprestimo parametros)
		{
            var emprestimos = from emprestimo  in BD.Emprestimos 
                              where parametros.Fechado  != null || emprestimo.Fechado == parametros.Fechado
                              where parametros.IdPessoa != null || emprestimo.Pessoa.Id == parametros.IdPessoa
                              where parametros.IdLivro  != null || emprestimo.Livro.Id == parametros.IdLivro
                              where parametros.Atrasado != null || !emprestimo.Fechado && emprestimo.DataFechamento == null &&  emprestimo.FimDataEmprestimo > _relogio.Agora
                              select emprestimo;
            return await emprestimos.ToListAsync();
		}   

        public async Task<Emprestimo?> Obter(int id)
        {

            var emprestimo = await BD.Emprestimos.FindAsync(id);
            if (emprestimo == null)
                return emprestimo;
            await BD.Entry(emprestimo).Reference(l => l.Pessoa).LoadAsync();
            await BD.Entry(emprestimo).Reference(l => l.Livro).LoadAsync();
            return emprestimo;
        }
        public async Task EditarFimData(int idEmprestimo, DateTime NovaData)
        {
            Emprestimo emprestimo = await Obter(idEmprestimo)
                ?? throw new InvalidOperationException($"Emprestimo {idEmprestimo} não existe");
            emprestimo.FimDataEmprestimo =  NovaData;
            BD.Update(emprestimo);
            await BD.SaveChangesAsync();
        }
        public async Task Excluir(int idEmprestimo)
        {
            Emprestimo emprestimo = await Obter(idEmprestimo)
                ?? throw new InvalidOperationException($"Emprestimo {idEmprestimo} não existe");
            BD.Remove(emprestimo);
            await BD.SaveChangesAsync();
        }
        public async Task Fechar(ParamFecharEmprestimo parametros)
		{			
            Emprestimo emprestimo = await Obter(parametros.IdEmprestimo)
				?? throw new InvalidOperationException($"Não existe o emprestimo de id {parametros.IdEmprestimo}");
			Usuario UsuarioFechador = await repUsuarios.Obter(parametros.idUsuarioFechador)
				?? throw new InvalidOperationException($"Não é possivel fechar emprestimo porque usuario de id {{{parametros.idUsuarioFechador}}} não existe.");
			
            emprestimo.Fechado = true;
			emprestimo.DataFechamento = _relogio.Agora;
			emprestimo.Devolvido = parametros.Devolvido;
			emprestimo.UsuarioFechador = UsuarioFechador;
			if (parametros.Devolvido)
			{
				emprestimo.AtrasoJustificado = parametros.AtrasoJustificado;
				emprestimo.ExplicacaoAtraso = parametros.ExplicacaoAtraso;
			}

            BD.SaveChanges();
		}
 
	}
}
