namespace RaffleApi.Entities;

public class Entrant
{
    public int Id { get; set; }
    public string Gamertag { get; set; }

    public string NormalizedGamertag { get => Gamertag.ToUpper(); set => Gamertag.ToUpper(); }
}