namespace LivrEtec.Exceptions;

[Serializable]
public class NaoAutenticadoException : Exception
{
    public NaoAutenticadoException() { }

    public NaoAutenticadoException(string message)
        : base(message) { }
    public NaoAutenticadoException(Usuario usuario)
        : base($"Tentativa de fazer ação não autenticada com o usuario de id {{{usuario.Id}}} e login {{{usuario.Nome}}} ") { }
    public NaoAutenticadoException(string message, Exception inner) : base(message, inner) { }
    protected NaoAutenticadoException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
