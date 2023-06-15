using LivrEtec.GIB.Cliente.Services;
using LivrEtec.GIB.Services;
using LivrEtec.Services;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace LivrEtec.GIB.Cliente
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		    builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            builder.Services.AddSingleton<IdentidadeService>();
            builder.Services.AddScoped<GrpcChannelProvider>();
            builder.Services.AddScoped((serviceProvider) => {
                var identidade = serviceProvider.GetRequiredService<IdentidadeService>();
                return serviceProvider.GetRequiredService<GrpcChannelProvider>().GetGrpcChannel(identidade.TokenJWT);
            });
            return builder.Build();
        }
    }
}