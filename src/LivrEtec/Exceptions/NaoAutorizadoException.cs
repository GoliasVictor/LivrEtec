namespace LivrEtec.Exceptions
{

    [Serializable]
    public class NaoAutorizadoException : Exception
    {
        public NaoAutorizadoException() { }
        public NaoAutorizadoException(Usuario usuario, Permissao permissao)
            : base($"O usuario de id {{{usuario.Id}}} e login {{{usuario.Login}}}, não possue a permissão {permissao.Nome} ") { }
        public NaoAutorizadoException(string message, Exception inner) : base(message, inner) { }
        protected NaoAutorizadoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
