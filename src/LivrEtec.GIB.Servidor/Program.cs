using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using LivrEtec.Services;
using LivrEtec.Repositorios;
using LivrEtec.GIB.Servidor.Interceptors;
using LivrEtec.GIB.Servidor.Services;
using LivrEtec.Servidor.Repositorios;
using LivrEtec.Servidor.Services;
using LivrEtec.Servidor.BD;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc( options => {
    options.Interceptors.Add<ExceptionInterceptor>();
    options.Interceptors.Add<IdentidadeInterceptor>();
});
builder.WebHost.UseUrls();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

string strAuthKey =  builder.Configuration["AuthKey"];
byte[] authKey = Encoding.ASCII.GetBytes(strAuthKey);
builder.Services.AddSingleton<AuthKeyProvider>(new AuthKeyProvider(authKey));
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options => {
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

app.Run();
