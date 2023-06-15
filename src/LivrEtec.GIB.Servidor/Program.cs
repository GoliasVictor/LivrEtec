using LivrEtec;
using LivrEtec.GIB.Servidor.Interceptors;
using LivrEtec.GIB.Servidor.Services;
using LivrEtec.Models;
using LivrEtec.Repositorios;
using LivrEtec.Servidor.BD;
using LivrEtec.Servidor.Repositorios;
using LivrEtec.Servidor.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Formatting.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
	options.Interceptors.Add<ExceptionInterceptor>();
	options.Interceptors.Add<IdentidadeInterceptor>();
});
builder.WebHost.UseUrls();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

string strAuthKey = builder.Configuration["AuthKey"];
byte[] authKey = Encoding.ASCII.GetBytes(strAuthKey);
builder.Services.AddSingleton<AuthKeyProvider>(new AuthKeyProvider(authKey));
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(authKey),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((context, services, configuration) =>
{
	configuration
		.ReadFrom.Configuration(context.Configuration)
		.ReadFrom.Services(services)
		.Enrich.FromLogContext()
		.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:w3}] {SourceContext}: {Message}{NewLine}{Exception}")
		.WriteTo.File(Path.Combine("./", "logs/txt/txt-.log"),
			rollingInterval: RollingInterval.Day,
			rollOnFileSizeLimit: true)
		.WriteTo.File(new JsonFormatter(), "logs/json/json-.log",
			rollingInterval: RollingInterval.Day,
			rollOnFileSizeLimit: true);
});

builder.Services.AddDbContextFactory<PacaContext>((options) =>
{
	var strConexao = builder.Configuration.GetConnectionString("MySql");
	options.UseMySql(strConexao, ServerVersion.AutoDetect(strConexao));
});
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.Services.AddGrpc();

builder.Services.AddDbContextFactory<PacaContext>(( options )=>{
    var strConexao = builder.Configuration.GetConnectionString("MySql");
    options.UseMySql(strConexao, ServerVersion.AutoDetect(strConexao));
});
builder.Services.AddSingleton<IRelogio, RelogioSistema>();
builder.Services.AddScoped<PacaContext>();
builder.Services.AddScoped<IRepUsuarios, RepUsuarios>();
builder.Services.AddScoped<IRepTags, RepTags>();
builder.Services.AddScoped<IRepAutores, RepAutores>();
builder.Services.AddScoped<IRepLivros, RepLivros>();
builder.Services.AddScoped<IRepPessoas, RepPessoas>();
builder.Services.AddScoped<IRepEmprestimos, RepEmprestimos>();

builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IAutorizacaoService, AutorizacaoService>();
builder.Services.AddScoped<IIdentidadeService, IdentidadeService>();
builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();
builder.Services.AddScoped<ILivrosService, LivrosService>();
builder.Services.AddScoped<ITagsService, TagsService>();
builder.Services.AddApplicationInsightsTelemetry();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<LivrosServiceRPC>();
app.MapGrpcService<GerenciamentoSessao>();
app.MapGrpcService<EmprestimoServiceRPC>();
app.MapGrpcService<TagsServiceRPC>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
	
using (var scope = app.Services.CreateScope()){
	using var BD = scope.ServiceProvider.GetRequiredService<PacaContext>();
	var logger = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<PacaContext>>();
	logger.LogTrace("Verificando se bando de dados existe...");
	
	if(BD.Database.EnsureCreated()) {
		logger.LogInformation("Banco de dados foi criado");
	}
	if(!BD.Usuarios.Any()){
		logger.LogInformation("Nenhum usuario cadastrado no sistema, criando admin...");
		var cargoAdmin = new Cargo()
		{
			Nome = "Admin",
			Permissoes = Permissoes.TodasPermissoes.ToList()
		};
		var admin = new Usuario()
		{
			Id = 1,
			Login = "admin",
			Nome = "admin",
			Senha = AutenticacaoService.GerarHahSenha(1, app.Configuration["SenhaPadraoAdmin"]),
			Cargo = cargoAdmin
		};
		BD.Add(cargoAdmin);
		BD.Add(admin);
		BD.SaveChanges();
		logger.LogInformation("admin criado");
	}

}

app.Run();
