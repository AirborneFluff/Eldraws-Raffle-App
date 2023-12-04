using System.Text.Json;
using RaffleApi.Helpers;
using RaffleApi.Interfaces;

namespace RaffleApi.Extensions;

public static class HttpResponseExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, IPagedList list)
    {
        var pageHeader = new PaginationHeader(list);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        response.Headers.Add("Pagination", JsonSerializer.Serialize(pageHeader, options));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}