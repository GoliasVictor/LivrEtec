namespace LivrEtec.GIB.Servidor;
class ConfiguracaoServidorGIB 
{
	public ConfiguracaoServidorGIB(string strConexaoMySQL)
	{
		StrConexaoMySQL = strConexaoMySQL;
	}

	public string StrConexaoMySQL { get; set; }
}