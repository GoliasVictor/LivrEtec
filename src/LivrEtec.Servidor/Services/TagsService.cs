using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LivrEtec.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor;
public sealed class TagsService : ITagsService
{
	readonly ILogger<TagsService>? Logger;
	readonly IIdentidadeService identidadeService;
	readonly IRepTags repTags;
	public TagsService(
		IRepTags repTags,
		IIdentidadeService identidadeService,
		ILogger<TagsService>? logger
	)
	{
		this.repTags = repTags;
		this.identidadeService = identidadeService;
		Logger = logger;
	}

	public async Task<IEnumerable<Tag>> BuscarAsync(string nome)
	{
		await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Tag.Visualizar);
		var tags = await repTags.BuscarAsync(nome);
		Logger?.LogInformation("Tags buscadas, Detalhes: busca={{{nome}}};",nome);
		return tags;
	}

	public async Task EditarAsync(Tag tag)
	{
		await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Tag.Editar);
		await repTags.EditarAsync(tag);
		Logger?.LogInformation("Tag Editada, Detalhes: nome={{{nome}}};", tag.Nome);
	}

	public async Task<Tag?> ObterAsync(int id)
	{
		await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Tag.Visualizar);
		var tag = await repTags.ObterAsync(id);
		Logger?.LogInformation("Tag obtida, Detalhes: id={{{id}}};", id);
		return tag;
	}

	public async Task<int> RegistrarAsync(Tag tag)
	{
		await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Tag.Criar);
		var id = await repTags.RegistrarAsync(tag);
		Logger?.LogInformation("Tag registrado, Detalhes: tag={{{tag}}};", tag);
            return id;
	}

	public async Task RemoverAsync(int id)
	{
		await identidadeService.ErroSeNaoAutorizadoAsync(Permissoes.Tag.Excluir);
		await repTags.RemoverAsync(id);
		Logger?.LogInformation("Tag excluida, Detalhes: id={{{id}}};", id);
	}
}