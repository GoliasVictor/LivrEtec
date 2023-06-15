using LivrEtec.GIB.Cliente.Services;
using LivrEtec.GIB.Services;
using LivrEtec.Services;
using Microsoft.AspNetCore.Components.Authorization;
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
            Preferences.Set("UrlAPI", "http://localhost:5259");
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddSingleton<IConfiguracaoService, ConfiguracaoService>();
            builder.Services.AddSingleton<IIdentidadeService, IdentidadeService>();
            builder.Services.AddSingleton<GrpcChannelProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider, LEAuthenticationStateProvider>();
            builder.Services.AddScoped((serviceProvider) => {
                return serviceProvider.GetRequiredService<GrpcChannelProvider>().GetGrpcChannel();
            });

            return builder.Build();
        }
    }
}