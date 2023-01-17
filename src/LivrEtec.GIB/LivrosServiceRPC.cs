﻿using LivrEtec.GIB;
using RPC = LivrEtec.GIB.RPC;
using Microsoft.Extensions.Logging;
using LivrEtec.GIB.RPC;
using Grpc.Core;

namespace LivrEtec.GIB
{
    public sealed class LivrosServiceRPC:  ILivrosService
    {
        readonly ILogger<LivrosServiceRPC> logger;
        readonly RPC::Livros.LivrosClient livrosClientRPC;
        public LivrosServiceRPC(RPC::Livros.LivrosClient livrosClientRPC, ILogger<LivrosServiceRPC> logger)
        {
            this.livrosClientRPC = livrosClientRPC;
            this.logger = logger;
        }

        public async Task Editar(Livro livro)
        {
            _ = livro ?? throw new ArgumentNullException(nameof(livro));
            if(livro.Tags.Any((t)=> t is null))
                throw new InvalidDataException("tag nula");

            livro.Tags ??= new();
            try{
                await livrosClientRPC.EditarAsync(livro);
            }
            catch(RpcException ex){
               throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

        public async Task<Livro?> Obter(int id)
        {
            try{
                return await livrosClientRPC.ObterAsync(new IdLivro() { Id = id });
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex); 
            }
        }

        public async Task Registrar(Livro livro)
        { 
            if(livro is not null){
                livro.Tags ??= new();
                livro.Autores ??= new();    
            }
            Validador.ErroSeInvalido(livro);
            if (string.IsNullOrWhiteSpace(livro.Nome) || livro.Id < 0)
                throw new InvalidDataException();
            try{
                await livrosClientRPC.RegistrarAsync(livro);
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }

		}


		public async Task Remover(int id)
        {
            try{
                await livrosClientRPC.RemoverAsync(new IdLivro(){ Id = id});
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

        public async Task<IEnumerable<Livro>> Buscar(string nome, string nomeAutor, IEnumerable<int>? idTags)
        {
            nome ??= "";
            nomeAutor ??= "";
            idTags ??= new List<int>();
            try{
			    ListaLivros listaLivros = await livrosClientRPC.BuscarAsync(new ParamBusca() { NomeLivro = nome, NomeAutor = nomeAutor, IdTags = { idTags }});
			    return listaLivros.Livros.Select(l=> (Livro)l!);
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
        }

	}
}