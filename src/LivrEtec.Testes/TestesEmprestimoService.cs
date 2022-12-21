using Xunit;
using LivrEtec.Servidor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LivrEtec.Testes;
using Xunit.Abstractions;
using LivrEtec.Testes.Stubs;
using LivrEtec.Testes.Doubles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LivrEtec.Servidor.Testes
{
    public class TestesEmprestimoService : IClassFixture<ConfiguradorTestes>
    {
        readonly BDUtil BDU;
        readonly IEmprestimoService emprestimoService;
        readonly IRelogio relogio;
        const int ID_PESSOA = 1;
        const int ID_ALUNO = 2 ;
        const int ID_LIVRO_DISPONIVEL = 1;

        public TestesEmprestimoService(ConfiguradorTestes configurador, ITestOutputHelper output)
        {
            BDU = new BDUtil(configurador, configurador.CreateLoggerFactory(output))
            {
                Autores = new Autor[]{
                    new Autor(1, "J. R. R. Tolkien"),
                    new Autor(2, "Friedrich Engels")
                },

                Tags = new Tag[]{
                    new Tag(1,"Aventura"),
                    new Tag(2,"Fantasia"),
                    new Tag(3,"Politica")
                }
            };
            BDU.Livros = new[]{
                new Livro {
                    Id = ID_LIVRO_DISPONIVEL,
                    Nome = "O Capital",
                    Arquivado = false,
                    Autores = { BDU.gAutor(2), BDU.gAutor(1), },
                    Tags = { BDU.gTag(1), BDU.gTag(3) },
                    Descricao = "É tudo nosso",
                    Quantidade = 2
                },
                new Livro {
                    Id = 3,
                    Nome = "A Revolução dos Bixos",
                    Arquivado = false,
                    Autores = { BDU.gAutor(2)},
                    Tags = { BDU.gTag(2) },
                    Descricao = "É tudo nosso",
                    Quantidade = 1
                }
            };

            BDU.Pessoas = new[]
            {
                new Pessoa(){
                    Id = ID_PESSOA,
                    Nome = "carlos",
                    Telefone = "13995789636"
                },
                new Aluno(){
                    Id = ID_ALUNO,
                    Nome = "joão",
                    RM="102433",
                    Telefone="13999822353"
                }
            };
            BDU.Emprestimos = new[]{
                new Emprestimo(){
                    Id= 1,
                    Livro = BDU.gLivro(1),
                    Pessoa = BDU.gPessoa(1),
                    Fechado = false,
                    AtrasoJustificado = false,
                    Comentario="",
                    DataDevolucao=new DateTime(2022,1,1),
                    FimDataEmprestimo=new DateTime(2022,1,1),
                    ExplicaçãoAtraso= null,
                    DataEmprestimo=new DateTime(2022, 1, 1)
                }
            };
            BDU.SalvarDados();
            var BD = BDU.CriarContexto();
            relogio = new RelogioStub(new DateTime(2020,10,10));
            var acervoService = new AcervoService(BD, configurador.CreateLogger<AcervoService>(output));
            emprestimoService = new EmprestimoService(
                acervoService, 
                new IIdentidadeServiceStub(),
                relogio,
                configurador.CreateLogger<EmprestimoService>(output)
            );
        }

        [Theory]
        [InlineData(ID_PESSOA)]
        [InlineData(ID_ALUNO)]
        public async Task AbrirAsync_ValidoAsync(int idPessoa)
        {
            var emprestimoEsperado = new Emprestimo() {
                Id = 2,
                Pessoa = BDU.gPessoa(idPessoa),
                Livro = BDU.gLivro(ID_LIVRO_DISPONIVEL),
                DataEmprestimo = relogio.Agora,
                Fechado = false,
                Comentario = null,
                DataDevolucao = null,
                AtrasoJustificado = null,
                ExplicaçãoAtraso = null,
                FimDataEmprestimo = relogio.Agora.AddDays(30),
                
            };
            var idEmprestimo = await emprestimoService.AbrirAsync(idPessoa, ID_LIVRO_DISPONIVEL);
            var BD = BDU.CriarContexto();
            var emprestimoAtual = (await BD.Emprestimos.FindAsync(idEmprestimo))!;
            BD.Entry(emprestimoAtual).Reference((e) => e.Pessoa).Load();
            BD.Entry(emprestimoAtual).Reference((e) => e.Livro).Load();
            Assert.NotNull(emprestimoAtual);
            AssertEmprestimoIgual(emprestimoEsperado, emprestimoAtual!);
            
        }

        private void AssertEmprestimoIgual(Emprestimo esperado, Emprestimo atual)
        {
            Assert.Equal(esperado.Id , atual.Id);
            Assert.Equal(esperado.Pessoa.Id , atual.Pessoa.Id);
            Assert.Equal(esperado.Livro.Id , atual.Livro.Id);
            Assert.Equal(esperado.DataEmprestimo , atual.DataEmprestimo);
            Assert.Equal(esperado.Fechado , atual.Fechado);
            Assert.Equal(esperado.Comentario , atual.Comentario);
            Assert.Equal(esperado.DataDevolucao , atual.DataDevolucao);
            Assert.Equal(esperado.AtrasoJustificado , atual.AtrasoJustificado);
            Assert.Equal(esperado.ExplicaçãoAtraso , atual.ExplicaçãoAtraso);
            Assert.Equal(esperado.FimDataEmprestimo, atual.FimDataEmprestimo); 
        }

        [Fact()]
        public void BuscarAsync_Resultado()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void ProrrogarAsnc_Resultado()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void RegistrarDevolucaoAsync_Resultado()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void RegistrarPerdaAsync_Resultado()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}