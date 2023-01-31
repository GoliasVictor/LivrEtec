using Grpc.Core.Interceptors;

namespace LivrEtec.GIB.Servidor.Interceptors;
public class ExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            throw ManipuladorException.ExceptionToRpcException(ex);
        }
    }
}