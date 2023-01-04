namespace LivrEtec.Testes;

public sealed class ConfiguracaoTeste
{
	public ConfiguracaoTeste(string strConexaoMySQL, string? urlGIBAPI = null)
	{
		UrlGIBAPI = urlGIBAPI;
		StrConexaoMySQL = strConexaoMySQL;
	}

	public string? UrlGIBAPI { get; set; }
	public string StrConexaoMySQL { get; set; }
	
}