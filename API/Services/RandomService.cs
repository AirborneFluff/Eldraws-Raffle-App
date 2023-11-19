namespace RaffleApi.Services;

public static class RandomService
{
    public static int GetRandomInteger(int max, int min = 0)
    {
        var rnd = new Random(Guid.NewGuid().GetHashCode());
        return rnd.Next(min, max);
    }
    
    public static int[] GetRandomIntegerList(int max, int min = 0, int count = 256)
    {
        Random rnd = new Random(Guid.NewGuid().GetHashCode());

        return Enumerable
            .Repeat(0, count)
            .Select(_ => rnd.Next(min, max))
            .ToArray();
    }
}