namespace RaffleApi.Configurations;

public sealed class RaffleMessageFactoryConfig
{
    public bool ShowWinners { get; set; } = false;
    public int? RollValue { get; set; } = null;
    public bool UseCustomEmojis { get; set; } = false;
}