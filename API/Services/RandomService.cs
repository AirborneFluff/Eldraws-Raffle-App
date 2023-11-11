namespace RaffleApi.Services;

public class RandomService
{
    private readonly HttpClient _http;

    public RandomService(HttpClient http)
    {
        _http = http;
    }

    public async Task<int> GetRandomInt(int max, int min = 0)
    {
        var result = await _http.GetStringAsync($"https://www.random.org/integers/?num=1&min={min}&max={max}&col=1&base=10&format=plain&rnd=new");
        return int.Parse(result);
    }
}