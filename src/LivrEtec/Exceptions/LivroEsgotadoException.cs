namespace LivrEtec.Exceptions;

[Serializable]
public class LivroEsgotadoException : Exception
{
	public int IdLivro;
	public LivroEsgotadoException() { }
	public LivroEsgotadoException(int idLivro, string message) : base(message)
	{
		Data.Add("idLivro", idLivro);
		 IdLivro = idLivro;
	}
	public LivroEsgotadoException(int idLivro, string message, Exception inner) : base(message, inner)
	{
		Data.Add("idLivro", idLivro);
		IdLivro = idLivro;
	}
	protected LivroEsgotadoException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
