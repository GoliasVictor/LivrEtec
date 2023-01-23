﻿using RPC = LivrEtec.GIB.RPC;
using Microsoft.Extensions.Logging;
using LivrEtec.GIB.RPC;
using Grpc.Core;
using static LivrEtec.GIB.RPC.Tag.Types;
using LivrEtec.Services;
using LivrEtec.Models;

namespace LivrEtec.GIB.Services
{
    public sealed class TagsServiceRPC : ITagsService
    {
        readonly ILogger<TagsServiceRPC> logger;
        readonly Tags.TagsClient tagsClientRPC;
        public TagsServiceRPC(Tags.TagsClient tagsClientRPC, ILogger<TagsServiceRPC> logger)
        {
            this.tagsClientRPC = tagsClientRPC;
            this.logger = logger;
        }
        public async Task<int> Registrar(Tag tag)
        {
            Validador.ErroSeInvalido(tag);
            try
            {
                return (await tagsClientRPC.RegistrarAsync(tag)).Id;
            }
            catch (RpcException ex)
            {
                throw ManipuladorException.RpcExceptionToException(ex);
            }

        }

        public async Task Editar(Tag tag)
        {
            _ = tag ?? throw new ArgumentNullException(nameof(tag));
            try
            {
                await tagsClientRPC.EditarAsync(tag);
            }
            catch (RpcException ex)
            {
                throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

        public async Task<Tag?> Obter(int id)
        {
            try
            {
                return await tagsClientRPC.ObterAsync(new IdTag() { Id = id });
            }
            catch (RpcException ex)
            {
                throw ManipuladorException.RpcExceptionToException(ex);
            }
        }


        public async Task Remover(int id)
        {
            try
            {
                await tagsClientRPC.RemoverAsync(new IdTag() { Id = id });
            }
            catch (RpcException ex)
            {
                throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

        public async Task<IEnumerable<Tag>> Buscar(string nome)
        {
            nome ??= "";
            try
            {
                ListaTags listaTags = await tagsClientRPC.BuscarAsync(new BuscarRequest() { Nome = nome });
                return listaTags.Tags.Select(l => (Tag)l!);
            }
            catch (RpcException ex)
            {
                throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

    }
}