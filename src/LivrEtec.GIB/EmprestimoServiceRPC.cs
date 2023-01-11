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

		public async Task<int> AbrirAsync(int idPessoa, int idlivro)
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

		public Task<IEnumerable<Emprestimo>> BuscarAsync(ParamBuscaEmprestimo parametros)
		{
			throw new NotImplementedException();
		}

		public async Task DevolverAsync(int idEmprestimo, bool? AtrasoJustificado = null, string? ExplicacaoAtraso = null)
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

		public async Task ProrrogarAsnc(int idEmprestimo, DateTime novaData)
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

		public async Task RegistrarPerdaAsync(int idEmprestimo)
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
        public async Task ExcluirAsync(int idEmprestimo)
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