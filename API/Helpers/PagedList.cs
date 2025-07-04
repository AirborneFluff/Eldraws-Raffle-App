﻿using Microsoft.EntityFrameworkCore;
using RaffleApi.Interfaces;

namespace RaffleApi.Helpers;

public class PagedList<T> : List<T>, IPagedList
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = count;
        AddRange(items);
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, PaginationParams pageParams)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageParams.PageNumber - 1) * pageParams.PageSize).Take(pageParams.PageSize).ToListAsync();

        return new PagedList<T>(items, count, pageParams.PageNumber, pageParams.PageSize);
    }
}