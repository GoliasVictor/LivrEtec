namespace LivrEtec.Testes.Doubles
{
    class RelogioStub : IRelogio
    {
        public RelogioStub(DateTime agora)
        {
            Agora = agora;
        }

        public DateTime Agora { get; set; }

    }
}
