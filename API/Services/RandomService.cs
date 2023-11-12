namespace RaffleApi.Services;

public static class RandomService
{
    public static int[] GetRandomIntegerList(int max, int min = 0, int count = 256)
    {
        Random rnd = new Random(Guid.NewGuid().GetHashCode());

        return Enumerable
            .Repeat(0, count)
            .Select(i => rnd.Next(min, max))
            .ToArray();
    }
}