using System.Text;
using Microsoft.Extensions.Configuration;

namespace LivrEtec.Testes;

public static class Configuracao
{
	static Configuracao()
	{
		string? AppSettingsJsonPath =  Environment.GetEnvironmentVariable("APP_SETTINGS_JSON_PATH");

		if(AppSettingsJsonPath is null){
			Console.WriteLine("Arquvio de configuração appsettings.json não definido, sera usado ./appsettings.json por padrão");
			AppSettingsJsonPath = "./appsettings.json";
		}
		var config = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddJsonFile(AppSettingsJsonPath)
			.Build();
		UrlGIBAPI =  config["UrlGIBAPI"];
		if(UrlGIBAPI != null){
			StrConexaoMySQL =  config["StrConexaoMySQL"] ?? throw new Exception("Defina um valor de StrConexaoMySQL nas configurações");
			string strAuthKey =  config["AuthKey"] ??  throw new Exception("Configure a chave de autenticaço (AuthKey)") ;
			AuthKey = Encoding.ASCII.GetBytes(strAuthKey);
		}
	}

	public static readonly string? UrlGIBAPI;
	public static readonly string? StrConexaoMySQL;
	public static readonly byte[]? AuthKey;
	
	
}