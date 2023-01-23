#pragma warning disable CS1998 // O método assíncrono não possui operadores 'await' e será executado de forma síncrona
using LivrEtec.Models;
using LivrEtec.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec.Testes.Doubles
{
    class IdentidadePermitidaStub : IIdentidadeService
    {
        public IdentidadePermitidaStub()
        {
            Usuario = new Usuario()
            {
                Nome = "Usuario de teste"
            };
        }

        public IdentidadePermitidaStub(Usuario? usuario)
        {
            Usuario = usuario;

        }

        public int IdUsuario => Usuario!.Id;

        public Usuario? Usuario { get; set; }

        public bool EstaAutenticado => true;

        public async Task AutenticarUsuario(string senha) { }

        public async Task AutenticarUsuario() { }

        public async Task DefinirUsuario(int idUsuario) { }

        public async Task<bool> EhAutorizado(Permissao permissao) => true;

        public async Task ErroSeNaoAutorizado(Permissao permissao) { }
    }
}
