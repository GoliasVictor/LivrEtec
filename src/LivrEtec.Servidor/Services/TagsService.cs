﻿using Microsoft.Extensions.Logging;

namespace LivrEtec.Servidor.Services;
public sealed class TagsService : ITagsService
{
    private readonly ILogger<TagsService>? Logger;
    private readonly IIdentidadeService identidadeService;
    private readonly IRepTags repTags;
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

    public async Task<IEnumerable<Tag>> Buscar(string nome)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Tag.Visualizar);
        IEnumerable<Tag> tags = await repTags.Buscar(nome);
        Logger?.LogInformation("Tags buscadas, Detalhes: busca={{{nome}}};", nome);
        return tags;
    }

    public async Task Editar(Tag tag)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Tag.Editar);
        await repTags.Editar(tag);
        Logger?.LogInformation("Tag Editada, Detalhes: nome={{{nome}}};", tag.Nome);
    }

    public async Task<Tag?> Obter(int id)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Tag.Visualizar);
        Tag? tag = await repTags.Obter(id);
        Logger?.LogInformation("Tag obtida, Detalhes: id={{{id}}};", id);
        return tag;
    }

    public async Task<int> Registrar(Tag tag)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Tag.Criar);
        var id = await repTags.Registrar(tag);
        Logger?.LogInformation("Tag registrado, Detalhes: tag={{{tag}}};", tag);
        return id;
    }

    public async Task Remover(int id)
    {
        await identidadeService.ErroSeNaoAutorizado(Permissoes.Tag.Excluir);
        await repTags.Remover(id);
        Logger?.LogInformation("Tag excluida, Detalhes: id={{{id}}};", id);
    }
}