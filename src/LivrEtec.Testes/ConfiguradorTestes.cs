using Microsoft.Extensions.Configuration;

namespace LivrEtec.Testes;
public class ConfiguradorTestes
{
	public ConfiguracaoTeste Config;
	public ConfiguradorTestes()
	{
		string? AppSettingsJsonPath =  Environment.GetEnvironmentVariable("APP_SETTINGS_JSON_PATH");

		if(AppSettingsJsonPath is null){
			Console.WriteLine("Arquvio de configuração appsettings.json não definido");
			throw new Exception("Arquvio de configuração appsettings.json não definido");
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
	
		

	}
}