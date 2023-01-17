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

        public async Task<int> Abrir(int idPessoa, int idLivro)
        {
            await identidadeService.ErroSeNaoAutorizado(Permissoes.Emprestimo.Criar);

            var pessoa = await repPessoas.ObterObter(idPessoa) 
                ?? throw new InvalidOperationException($"Pessoa de id {{{idPessoa}}} não existe.");
            var livro = await repLivros.Obter(idLivro) 
                ?? throw new InvalidOperationException($"Livro de id {{{idPessoa}}} não existe.");
            
            var QtEmprestada = await repEmprestimos.ObterQuantidadeLivrosEmprestado(idLivro);
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
            var id = await repEmprestimos.Registrar(Emprestimo); 
            Logger?.LogInformation("Emprestimo {{{id}}} aberto", id);
            return id ;
        }

        public async Task<IEnumerable<Emprestimo>> Buscar(ParamBuscaEmprestimo parametros)
        {
            await identidadeService.ErroSeNaoAutorizado(Permissoes.Emprestimo.Visualizar);
            var emprestimos = await repEmprestimos.Buscar(parametros);
            Logger?.LogInformation("Foram buscado os emprestimso do usuario {{{idPessoa}}} e livro {{{idLivro}}}",parametros.IdPessoa, parametros.IdLivro);
            return emprestimos;
        }

        public async Task Prorrogar(int idEmprestimo, DateTime novaData)
        {
            await identidadeService.ErroSeNaoAutorizado(Permissoes.Emprestimo.Editar);
            await repEmprestimos.EditarFimData(idEmprestimo, novaData);
            Logger?.LogInformation("Data do livro {{{idEmprestimo}}}, foi modificada para {{{novaData}}}",idEmprestimo, novaData);
            
        }
        public Task Devolver(int idEmprestimo, bool? AtrasoJustificado = null, string? ExplicacaoAtraso = null){
            return FecharAsync(new ParamFecharEmprestimo(){
                IdEmprestimo = idEmprestimo, 
                Devolvido = true,
                AtrasoJustificado = AtrasoJustificado,
                ExplicacaoAtraso = ExplicacaoAtraso
            });
        }
        public Task RegistrarPerda(int idEmprestimo){
            return FecharAsync(new ParamFecharEmprestimo(){
                IdEmprestimo = idEmprestimo, 
                Devolvido = false,
            });
        }
        private async Task FecharAsync(ParamFecharEmprestimo parametros)
        {
            await identidadeService.ErroSeNaoAutorizado(Permissoes.Emprestimo.Fechar);
            parametros.idUsuarioFechador = identidadeService.IdUsuario;
            await repEmprestimos.Fechar(parametros);
            
            Logger?.LogInformation("O emprestimo {{{idEmprestimo}}} foi devolvido", parametros.IdEmprestimo);
        }

        public async Task Excluir(int idEmprestimo)
        {
            await identidadeService.ErroSeNaoAutorizado(Permissoes.Emprestimo.Excluir);
            await repEmprestimos.Excluir(idEmprestimo);
            
            Logger?.LogInformation("O emprestimo {{{idEmprestimo}}} foi excluido", idEmprestimo);
        }
    }
}
