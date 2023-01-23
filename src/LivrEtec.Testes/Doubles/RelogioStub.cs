namespace LivrEtec.Testes.Doubles;

internal class RelogioStub : IRelogio
{
    public RelogioStub(DateTime agora)
    {
        Agora = agora;
    }

    public DateTime Agora { get; set; }

}
