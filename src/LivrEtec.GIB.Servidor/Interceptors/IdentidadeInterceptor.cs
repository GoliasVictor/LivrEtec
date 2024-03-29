using Grpc.Core.Interceptors;
using System.Security.Claims;

namespace LivrEtec.GIB.Servidor.Interceptors;
public class IdentidadeInterceptor : Interceptor
{
    private readonly IIdentidadeService IdentidadeService;

    public IdentidadeInterceptor(IIdentidadeService identidadeService)
    {
        IdentidadeService = identidadeService;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {

        ClaimsPrincipal user = context.GetHttpContext().User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await IdentidadeService.DefinirUsuario(id);
            await IdentidadeService.AutenticarUsuario();
        }

        return await continuation(request, context);
    }
}