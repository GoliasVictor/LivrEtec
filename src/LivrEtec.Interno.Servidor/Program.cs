using LivrEtec; 
using LivrEtec.Interno.Servidor;
using LivrEtec.Servidor;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

builder.Services.AddGrpc();

var Config = builder.Configuration.GetSection("ConfiguracaoInterna").Get<ConfiguracaoServidorInterno>();

builder.Services.AddSingleton<IConfiguracao>(Config);
builder.Services.AddScoped<PacaContext>();
builder.Services.AddLogging(configure => {
    configure.AddConsole();
});
builder.Services.AddScoped<IAcervoService, AcervoService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<LivroServiceRPC>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
