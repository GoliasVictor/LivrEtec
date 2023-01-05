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
        readonly ILogger<EmprestimoService>? Logger;
        readonly IIdentidadeService identidadeService;
        readonly IRepEmprestimos repEmprestimos;
        readonly IRepPessoas repPessoas; 
        readonly IRepLivros repLivros;
        public EmprestimoService(
            IRepEmprestimos repEmprestimos,
            IRepPessoas repPessoas, 
            IRepLivros repLivros,
            IIdentidadeService identidadeService, 
            IRelogio relogio, 
            ILogger<EmprestimoService>? logger
        )
        {
            this.repEmprestimos = repEmprestimos;
            this.repPessoas = repPessoas;
            this.repLivros = repLivros;
            this.identidadeService = identidadeService;
            this.relogio = relogio;
            Logger = logger;
        }

        public async Task<int> AbrirAsync(int idPessoa, int idLivro)
        {
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Emprestimo.Criar);

            var pessoa = await repPessoas.ObterAsync(idPessoa) 
                ?? throw new InvalidOperationException($"Pessoa de id {{{idPessoa}}} não existe.");
            var livro = await repLivros.GetAsync(idLivro) 
                ?? throw new InvalidOperationException($"Livro de id {{{idPessoa}}} não existe.");
            
            var QtEmprestada = await repEmprestimos.ObterQuantidadeLivrosEmprestadoAsync(idLivro);
            var LivrosDisponiveis = livro.Quantidade - QtEmprestada;
            if (LivrosDisponiveis <= 0)
                throw new LivroEsgotadoException(livro.Id, $"Não é possivel abrir emprestimo, livro {{{livro.Id}}}");
            
            var Emprestimo = new Emprestimo() {
                Pessoa = pessoa,
                Livro = livro,
                UsuarioCriador = identidadeService.Usuario!,
                DataEmprestimo = relogio.Agora,
                FimDataEmprestimo = relogio.Agora.AddDays(30),
            };
            var id = await repEmprestimos.RegistrarAsync(Emprestimo); 
            Logger?.LogInformation("Emprestimo {{{id}}} aberto", id);
            return id ;
        }

        public async Task<IEnumerable<Emprestimo>> BuscarAsync(ParamBuscaEmprestimo parametros)
        {
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Emprestimo.Visualizar);
            var emprestimos = await repEmprestimos.BuscarAsync(parametros);
            Logger?.LogInformation("Foram buscado os emprestimso do usuario {{{idPessoa}}} e livro {{{idLivro}}}",parametros.IdPessoa, parametros.IdLivro);
            return emprestimos;
        }

        public async Task ProrrogarAsnc(int idEmprestimo, DateTime novaData)
        {
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Emprestimo.Editar);
            await repEmprestimos.EditarFimData(idEmprestimo, novaData);
            Logger?.LogInformation("Data do livro {{{idEmprestimo}}}, foi modificada para {{{novaData}}}",idEmprestimo, novaData);
            
        }
        public Task DevolverAsync(int idEmprestimo, bool? AtrasoJustificado = null, string? ExplicacaoAtraso = null){
            return FecharAsync(new ParamFecharEmprestimo(){
                IdEmprestimo = idEmprestimo, 
                Devolvido = true,
                AtrasoJustificado = AtrasoJustificado,
                ExplicacaoAtraso = ExplicacaoAtraso
            });
        }
        public Task RegistrarPerdaAsync(int idEmprestimo){
            return FecharAsync(new ParamFecharEmprestimo(){
                IdEmprestimo = idEmprestimo, 
                Devolvido = false,
            });
        }
        private async Task FecharAsync(ParamFecharEmprestimo parametros)
        {
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Emprestimo.Fechar);
            parametros.idUsuarioFechador = identidadeService.IdUsuario;
            await repEmprestimos.FecharAsync(parametros);
            
            Logger?.LogInformation("O emprestimo {{{idEmprestimo}}} foi devolvido", parametros.IdEmprestimo);
        }

        public async Task ExcluirAsync(int idEmprestimo)
        {
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Emprestimo.Excluir);
            await repEmprestimos.Excluir(idEmprestimo);
            
            Logger?.LogInformation("O emprestimo {{{idEmprestimo}}} foi excluido", idEmprestimo);
        }
    }
}
