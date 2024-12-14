using Grpc.Core.Interceptors;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace LivrEtec.GIB.Interceptors;
public class IdentidadeInterceptor : IMiddleware
{
    private readonly IIdentidadeService IdentidadeService;

    public IdentidadeInterceptor(IIdentidadeService identidadeService)
    {
        IdentidadeService = identidadeService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Console.WriteLine(context.Request.Headers[HeaderNames.Authorization].ToString());

        ClaimsPrincipal user = context.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            Console.WriteLine("B");
            var id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            IdentidadeService.EstaAutenticado = true;
            IdentidadeService.Usuario = new Models.Usuario() { Id = id };
            await IdentidadeService.CarregarUsuario();
        }

        await next(context);
    }
}