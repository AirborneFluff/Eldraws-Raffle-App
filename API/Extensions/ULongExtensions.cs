namespace RaffleApi.Extensions;

public static class ULongExtensions
{
    public static ulong? ParseNullableULong(string? input)
    {
        if (input == null) return null;
        if (!ulong.TryParse(input, out var value)) return null;
        return value;
    }
}