using LivrEtec; 
using LivrEtec.GIB.Servidor;
using LivrEtec.Servidor;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

builder.Services.AddGrpc();

var Config = builder.Configuration.GetSection("ConfiguracaoInterna").Get<ConfiguracaoServidorGIB>();
builder.Services.AddDbContextFactory<PacaContext>(( _ )=>{

});
builder.Services.AddSingleton<IConfiguracao>(Config);
builder.Services.AddTransient<PacaContext>();
builder.Services.AddLogging(configure => {
    configure.AddConsole();
});
builder.Services.AddTransient<IAcervoService, AcervoService>();
builder.Services.AddApplicationInsightsTelemetry();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<LivroServiceRPC>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
