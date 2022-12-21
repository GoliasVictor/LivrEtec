namespace LivrEtec
{
    public sealed class RelogioSistema : IRelogio
    {
        public DateTime Agora => DateTime.Now;
    }
}