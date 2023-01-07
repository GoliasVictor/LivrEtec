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
    public sealed class LivrosService : ILivrosService
    {
        readonly ILogger<LivrosService>? Logger;
        readonly IIdentidadeService identidadeService;
        readonly IRepLivros repLivros;
        public LivrosService(
            IRepLivros repLivros,
            IIdentidadeService identidadeService, 
            ILogger<LivrosService>? logger
        )
        {
            this.repLivros = repLivros;
            this.identidadeService = identidadeService;
            Logger = logger;
        }

		public async Task<IEnumerable<Livro>> BuscarAsync(string nome, string nomeAutor, IEnumerable<int>? idTags = null)
		{
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Livro.Visualizar);
            var livros =  await repLivros.BuscarAsync(nome, nomeAutor, idTags);
            Logger?.LogInformation("Livros buscados, Detalhes: nome={{{nome}}}; nomeAutor={{{nomeAutor}}}; idTags={{{idTags}}}", 
                nome, 
                nomeAutor, 
                idTags != null ? string.Join(",", idTags ) : null 
            );
            return livros;
		}

		public async Task EditarAsync(Livro livro)
		{
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Livro.Editar);
            await repLivros.EditarAsync(livro);
            Logger?.LogInformation(1,"Livro Editado, Detalhes: livro={{{nome}}};", livro);
		}

		public async Task<Livro?> GetAsync(int id)
		{
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Livro.Visualizar);
            var livro = await repLivros.GetAsync(id);
            Logger?.LogInformation(1,"Livro obtido, Detalhes: id={{{id}}};", id);
            return livro;
		}

		public async Task RegistrarAsync(Livro livro)
		{
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Livro.Criar);
            await repLivros.RegistrarAsync(livro);
            Logger?.LogInformation(1,"Livro registrado, Detalhes: livro={{{livro}}};", livro);
		}

		public async Task RemoverAsync(int id)
		{
            await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Livro.Excluir);
            await repLivros.RemoverAsync(id);
            Logger?.LogInformation(1,"Livro excluid, Detalhes: id={{{id}}};", id);
		}
	}
}
