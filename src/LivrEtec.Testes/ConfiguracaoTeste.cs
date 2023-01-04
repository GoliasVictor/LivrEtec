namespace LivrEtec.Testes;

public sealed class ConfiguracaoTeste
{
	public ConfiguracaoTeste(string urlGIBAPI, string strConexaoMySQL)
	{
		UrlGIBAPI = urlGIBAPI;
		StrConexaoMySQL = strConexaoMySQL;
	}

	public string UrlGIBAPI { get; set; }
	public string StrConexaoMySQL { get; set; }
	
}