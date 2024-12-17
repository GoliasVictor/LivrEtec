using System.Security.Claims;

namespace LivrEtec.GIB.Interceptors;
public class IdentidadeMiddleware : IMiddleware
{
    private readonly IIdentidadeService IdentidadeService;

    public IdentidadeMiddleware(IIdentidadeService identidadeService)
    {
        IdentidadeService = identidadeService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        ClaimsPrincipal user = context.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            IdentidadeService.EstaAutenticado = true;
            IdentidadeService.Usuario = new Models.Usuario() { Id = id };
            await IdentidadeService.CarregarUsuario();
        }

        await next(context);
    }
}