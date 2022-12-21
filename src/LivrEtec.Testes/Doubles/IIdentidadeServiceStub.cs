#pragma warning disable CS1998 // O método assíncrono não possui operadores 'await' e será executado de forma síncrona
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec.Testes.Stubs
{
    class IIdentidadeServiceStub : IIdentidadeService
    {
        public int IdUsuario => 0;

        public Usuario? Usuario => new Usuario() {  };

        public bool EstaAutenticado => true;

        public async Task AutenticarUsuarioAsync(string senha){ }

        public async Task DefinirUsuarioAsync(int idUsuario){ }

        public async Task<bool> EhAutorizadoAsync(Permissao permissao) => true;

        public async Task ErroSeNaoAutorizadoAsync(Permissao permissao){ }
    }
}
