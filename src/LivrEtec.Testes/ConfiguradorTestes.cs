using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LivrEtec.Testes;
public sealed class ConfiguradorTestes
{
	public ConfiguracaoTeste Config;
	public ILoggerFactory loggerFactory = LoggerFactory.Create((lb)=> { 
		lb.AddConsole();
		lb.AddFilter((_,_, logLevel)=> logLevel >= LogLevel.Information);
	});
	public ConfiguradorTestes()
	{ 
		string? AppSettingsJsonPath =  Environment.GetEnvironmentVariable("APP_SETTINGS_JSON_PATH");

		if(AppSettingsJsonPath is null){
			Console.WriteLine("Arquvio de configuração appsettings.json não definido, sera usado ./appsettings.json por padrão");
			AppSettingsJsonPath = "./appsettings.json";
		}
		var config = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddJsonFile(AppSettingsJsonPath)
			.Build()
			.Get<ConfiguracaoTeste>();
		if(config is null){
			Console.WriteLine("Ocorreu algum erro na configuração");
			throw new Exception("Ocorreu algum erro na configuração");
		}
		Config = config;  
	}
}