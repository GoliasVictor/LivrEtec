using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LivrEtec.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor
{
    public sealed class EmprestimoService : IEmprestimoService
    {
        readonly IRelogio relogio;
        readonly IAcervoService acervoService;
        readonly ILogger? Logger;

        public IIdentidadeService identidadeService { get; set; }
        public EmprestimoService(IAcervoService acervoService, IIdentidadeService identidadeService, IRelogio relogio, ILogger? logger)
        {
            this.identidadeService = identidadeService;
            this.acervoService = acervoService;
            this.relogio = relogio;
            Logger = logger;
        }

        public async Task<int> AbrirAsync(int idPessoa, int idLivro)
        {
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Emprestimo.Criar);

            var pessoa = await acervoService.Pessoas.ObterAsync(idPessoa) ?? throw new InvalidOperationException($"Pessoa de id {{{idPessoa}}} não existe.");
            var livro = await acervoService.Livros.GetAsync(idLivro) ?? throw new InvalidOperationException($"Livro de id {{{idPessoa}}} não existe.");

            var QtEmprestada = await acervoService.Emprestimos.ObterQuantidadeLivrosEmprestadoAsync(idLivro);
            var LivrosDisponiveis = livro.Quantidade - QtEmprestada;
            if (LivrosDisponiveis <= 0)
                throw new LivroEsgotadoException(livro.Id, $"Não é possivel abrir emprestimo, livro {{{livro.Id}}}");
            
            var Emprestimo = new Emprestimo() {
                Pessoa = pessoa,
                Livro = livro,
                DataEmprestimo = relogio.Agora,
                DataDevolucao = relogio.Agora.AddDays(30),
            };
            var id = await acervoService.Emprestimos.RegistrarAsync(Emprestimo); 
            Logger?.LogInformation("Emprestimo {{{resultado}}} aberto", id);
            return id ;
        }

        public Task<IEnumerable<Emprestimo>> BuscarAsync(int idPessoa, bool aberto, bool atrasado)
        {
            throw new NotImplementedException();
        }

        public Task ProrrogarAsnc(int idEmprestimo, DateTime novaData)
        {
            throw new NotImplementedException();
        }

        public Task RegistrarDevolucaoAsync(int idEmprestimo)
        {
            throw new NotImplementedException();
        }

        public Task RegistrarPerdaAsync(int idEmprestimo, string justificativa)
        {
            throw new NotImplementedException();
        }
    }
}
