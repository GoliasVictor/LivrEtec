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


builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddDbContextFactory<PacaContext>(( options )=>{
    var strConexao = builder.Configuration.GetConnectionString("MySql");
    options.UseMySql(strConexao, ServerVersion.AutoDetect(strConexao));
});
builder.Services.AddSingleton<IRelogio,RelogioSistema>(); 
builder.Services.AddScoped<PacaContext>();
builder.Services.AddLogging(configure => {
    configure.AddConsole();
}); 
builder.Services.AddScoped<IRepUsuarios, RepUsuarios>();
builder.Services.AddScoped<IRepAutores, RepAutores>();
builder.Services.AddScoped<IRepLivros, RepLivros>();
builder.Services.AddScoped<IRepPessoas, RepPessoas>();
builder.Services.AddScoped<IRepEmprestimos, RepEmprestimos>();

builder.Services.AddScoped<IIdentidadeService,IdentidadeServiceRPC>();
builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();
builder.Services.AddApplicationInsightsTelemetry();
var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapGrpcService<LivroServiceRPC>();
app.MapGrpcService<EmprestimoServiceRPC>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
