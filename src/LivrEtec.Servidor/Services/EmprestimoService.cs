using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LivrEtec.Servidor
{
    sealed class EmprestimoService : IEmprestimoService
    {
        private IRelogio relogio;
        private IAcervoService acervoService;
        public EmprestimoService(IAcervoService acervoService, IRelogio relogio )
        {
            this.acervoService = acervoService;
            this.relogio = relogio;
        }

        public async Task<int> AbrirAsync(int idPessoa, int idLivro)
        { 
            var pessoa = await acervoService.Pessoas.ObterAsync(idPessoa);
            _ = pessoa ?? throw new InvalidOperationException($"Pessoa de id {{{idPessoa}}} não existe.");
            var livro = await acervoService.Livros.GetAsync(idLivro);
            _ = livro ?? throw new InvalidOperationException($"Livro de id {{{idPessoa}}} não existe.");

            //var quantidadeLivrosEmprestado = ObterQuantidadeLivrosEmprestado(idLivro, BD);
            //livro.Quantidade - quantidadeLivrosEmprestado;
            var Emprestimo = new Emprestimo()
            {
                Pessoa = pessoa,
                Livro = livro,
                DataDevolucao = relogio.Agora
            };
            return await acervoService.Emprestimos.RegistrarAsync(Emprestimo);
            
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
