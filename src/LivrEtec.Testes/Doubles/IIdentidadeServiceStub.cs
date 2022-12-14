#pragma warning disable CS1998 // O método assíncrono não possui operadores 'await' e será executado de forma síncrona
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec.Testes
{
    class IdentidadePermitidaStub : IIdentidadeService
    {
		public IdentidadePermitidaStub()
		{
            Usuario = new Usuario() {  
                Nome = "Usuario de teste"
            };
		}

		public IdentidadePermitidaStub(Usuario? usuario)
		{
			Usuario = usuario;

		}

		public int IdUsuario => Usuario!.Id;

        public Usuario? Usuario {get;set;} 

        public bool EstaAutenticado => true;

        public async Task AutenticarUsuarioAsync(string senha){ }

        public async Task DefinirUsuarioAsync(int idUsuario){ }

        public async Task<bool> EhAutorizadoAsync(Permissao permissao) => true;

        public async Task ErroSeNaoAutorizadoAsync(Permissao permissao){ }
    }
}
