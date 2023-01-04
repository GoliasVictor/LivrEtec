using LivrEtec; 
using LivrEtec.GIB.Servidor; 
using LivrEtec.Servidor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.Services.AddGrpc();

var Config = builder.Configuration.GetSection("ConfiguracaoInterna").Get<ConfiguracaoServidorGIB>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddDbContextFactory<PacaContext>(( options )=>{
    options.UseMySql(Config.StrConexaoMySQL, ServerVersion.AutoDetect(Config.StrConexaoMySQL));
});
builder.Services.AddLogging();
builder.Services.AddSingleton<IRelogio,RelogioSistema>(); 
builder.Services.AddTransient<PacaContext>();
builder.Services.AddLogging(configure => {
    configure.AddConsole();
}); 

builder.Services.AddScoped<IIdentidadeService,IdentidadeServiceRPC>();
builder.Services.AddTransient<IAcervoService, AcervoService>();
builder.Services.AddTransient<IEmprestimoService, EmprestimoService>();
builder.Services.AddApplicationInsightsTelemetry();
var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapGrpcService<LivroServiceRPC>();
app.MapGrpcService<EmprestimoServiceRPC>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
