namespace LivrEtec.GIB;

internal class ConfiguracaoServidorGIB
{
	public ConfiguracaoServidorGIB(string strConexaoMySQL)
	{
		StrConexaoMySQL = strConexaoMySQL;
	}

	public string StrConexaoMySQL { get; set; }
}