using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Xunit.Extensions;
using Xunit.Sdk;

namespace LivrEtec.Testes;

public sealed class ConfiguradorTestes
{
	public ConfiguracaoTeste Config;
    public ILoggerFactory CreateLoggerFactory(ITestOutputHelper output)
	{
		return LoggerFactory.Create((lb) => {
			lb.AddXUnit(output);
			lb.SetMinimumLevel(LogLevel.Information);
            //lb.AddFilter((_, _, logLevel) => logLevel >= LogLevel.Information);
            lb.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
        });
    }
    public ILogger<T> CreateLogger<T>(ITestOutputHelper output)
    {
        return CreateLoggerFactory(output).CreateLogger<T>();
    }
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