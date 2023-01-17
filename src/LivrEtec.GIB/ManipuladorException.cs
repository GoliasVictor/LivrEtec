using Grpc.Core;

namespace LivrEtec.GIB
{
    public static class ManipuladorException
    {


        public static Exception RpcExceptionToException(RpcException ex)
        {
            var Excecao = ex.Trailers.FirstOrDefault(p => p.Key == "excecao")?.Value;
            var Mensagem = ex.Trailers.FirstOrDefault(p => p.Key == "mensagem")?.Value;

            switch (Excecao)
            {
                case nameof(ArgumentNullException):
                    var NomeParametro = ex.Trailers.FirstOrDefault(p => p.Key == "NomeParametro")?.Value;
                    return new ArgumentNullException(Mensagem, ex);
                case nameof(InvalidDataException):
                    return new InvalidDataException(Mensagem, ex);
                case nameof(InvalidOperationException):
                    return new InvalidOperationException(Mensagem, ex);
                default:
                    return ex;
            }
        }
        public static RpcException ExceptionToRpcException(Exception ex)
        {
            if(ex is RpcException rpcEx)
				return rpcEx;
			var metadata = new Metadata(){
                { "excecao" , ex.GetType().Name },
            };
            switch (ex)
            {
                case InvalidDataException:
                    return new RpcException(new Status(StatusCode.InvalidArgument, $"Dados invalidos", ex), metadata);
                case ArgumentNullException ArgumentNull:
                    metadata.Add(nameof(ArgumentNull.ParamName), ArgumentNull.Message);
                    return new RpcException(new Status(StatusCode.InvalidArgument, $"{ArgumentNull.ParamName} e nulo", ex), metadata);
                case InvalidOperationException:
                    return new RpcException(new Status(StatusCode.FailedPrecondition, "Operacao Invalida", ex), metadata);
                case NaoAutorizadoException:
					return new RpcException(new Status(StatusCode.PermissionDenied, "Permissão Negada", ex), metadata);
                case NaoAutenticadoException:
					return new RpcException(new Status(StatusCode.Unauthenticated, "Não Autenticado", ex), metadata);
				default:
                    return new RpcException(new Status(StatusCode.Internal, "Erro interno", ex));
            }
        }
    }
}