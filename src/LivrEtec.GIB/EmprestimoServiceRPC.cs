using LivrEtec.GIB;
using RPC = LivrEtec.GIB.RPC;
using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;
using LivrEtec.GIB.RPC;
using Grpc.Core;
using static LivrEtec.GIB.RPC.Emprestimo.Types;

namespace LivrEtec.GIB
{
    public sealed class EmprestimoServiceRPC: IEmprestimoService
    {
        readonly ILogger<EmprestimoServiceRPC> logger;
        readonly RPC::Emprestimos.EmprestimosClient clientRPC;
        public EmprestimoServiceRPC(ILogger<EmprestimoServiceRPC> logger, RPC::Emprestimos.EmprestimosClient clientRPC)
        {
            this.clientRPC = clientRPC;
            this.logger = logger;
        }

		public async Task<int> Abrir(int idPessoa, int idlivro)
		{
            try{
                IdEmprestimo idEmprestimo = await clientRPC.AbrirAsync(new AbrirRequest(){
                    IdLivro = idlivro,
                    IdPessoa = idPessoa
                });
                return idEmprestimo.Id;
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
		}

		public Task<IEnumerable<Emprestimo>> Buscar(ParamBuscaEmprestimo parametros)
		{
			throw new NotImplementedException();
		}

		public async Task Devolver(int idEmprestimo, bool? AtrasoJustificado = null, string? ExplicacaoAtraso = null)
		{
            try{
				DevolverRequest request = new DevolverRequest(){ IdEmprestimo = idEmprestimo };
                if(AtrasoJustificado is not null)
                    request.AtrasoJustificado = AtrasoJustificado.Value;
                if(ExplicacaoAtraso is not null)
                    request.ExplicacaoAtraso = ExplicacaoAtraso;
				await clientRPC.DevolverAsync(request);
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
		}

		public async Task Prorrogar(int idEmprestimo, DateTime novaData)
		{

            try{
                await clientRPC.ProrrogarAsync(new ProrrogarRequest(){
                    IdEmprestimo = idEmprestimo,
                    NovaData = Timestamp.FromDateTime(novaData.ToUniversalTime())
                });
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
		}

		public async Task RegistrarPerda(int idEmprestimo)
		{
            try{
                await clientRPC.RegistrarPerdaAsync(new IdEmprestimo (){
                    Id = idEmprestimo,
                });
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
		}
        public async Task Excluir(int idEmprestimo)
		{
            try{

                await clientRPC.ExcluirAsync(new IdEmprestimo (){
                    Id = idEmprestimo,
                });
            }
            catch(RpcException ex){
                throw ManipuladorException.RpcExceptionToException(ex);
            }
		}
	}
}