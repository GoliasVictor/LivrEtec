using LivrEtec.Servidor;
using Grpc.Core.Interceptors;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using System.ComponentModel.DataAnnotations;
using LivrEtec.Services;
using LivrEtec.Models;
using LivrEtec.Testes.Utilitarios;

namespace LivrEtec.Testes;

[Collection("UsaBancoDeDados")]
public abstract class TestesTagsService<T> where T : ITagsService  
{
	const int ID_TAG_EXISTENTE = 1;
	protected abstract T tagsService { get;init;}
	protected readonly BDUtil BDU; 

	public static void AssertEhIgual<K>( IEnumerable<K> A, IEnumerable<K> B){
		Assert.Equal(new HashSet<K>(A),new HashSet<K>(B));
	}
    static void AssertTagIgual(Tag tagEsperada, Tag tagAtual)
    {
        Assert.Equal(tagEsperada.Nome, tagAtual.Nome);
        Assert.Equal(tagEsperada.Id, tagAtual.Id);
    }
	public TestesTagsService(ITestOutputHelper output, BDUtil bdu)
	{
		BDU = bdu;
		BDU.Tags = new Tag[]{
			new Tag(ID_TAG_EXISTENTE,"Aventura"),
			new Tag(2,"Fantasia"),
			new Tag(3,"Politica"),
			new Tag(4,"Literatura"),
			new Tag(5,"Sociologia"),
		};  
		BDU.SalvarDados();
	} 
	[Fact]
	public async Task Registrar_Valida(){
		var tagEsperada  = new Tag{
			Nome = "Matématica",
		};
		var id = await tagsService.Registrar(tagEsperada);
		var tagAtual = await BDU.gTagBanco(id);
	}
	[Fact]
	public async Task Obter_Valida(){
		int idTag = ID_TAG_EXISTENTE;
		var tagEsperada = BDU.gTag(idTag);

		var tagAtual = await tagsService.Obter(idTag);
		Assert.NotNull(tagAtual);
		AssertTagIgual(tagEsperada, tagAtual!);
	}
	[Fact]
	public async Task Buscar_Valida(){
		string nomeBusca = "li";
		var tagsEsperadas = new[] { 
			new Tag(3,"Politica"),
			new Tag(4,"Literatura"),
		}; 

		var tagsAtuais = await tagsService.Buscar(nomeBusca);

		Assert.NotNull(tagsAtuais);
		AssertEhIgual(tagsEsperadas, tagsAtuais);
	}

	[Fact]
	public async Task Editar_Valida(){
		int idTag = ID_TAG_EXISTENTE;
		var tagEsperada = new Tag{
			Id =  idTag,
			Nome="Aventura Medieval"
		};

		await tagsService.Editar(tagEsperada);
		
		var tagAtual = await BDU.gTagBanco(idTag);

		Assert.NotNull(tagAtual);
		AssertTagIgual(tagEsperada, tagAtual!);
	}

	[Fact]
	public async Task Excluir_Valida(){
		int idTag = ID_TAG_EXISTENTE;
	
		await tagsService.Remover(idTag);
		
		var tagAtual = await BDU.gTagBanco(idTag);
		Assert.Null(tagAtual);
	}
}
