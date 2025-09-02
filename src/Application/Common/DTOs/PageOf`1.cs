namespace SchoolTripApi.Application.Common.DTOs;

public sealed class PageOf<T>
{
    public PageOf(List<T> items, int pageNumber, int pageSize, int totalCount, int totalPages)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        HasPrevious = PageNumber > 1;
        HasNext = PageNumber < TotalPages;
    }

    public List<T> Items { get; private set; }
    public int TotalCount { get; private set; }
    public int PageNumber { get; }
    public int PageSize { get; private set; }
    public int TotalPages { get; }
    public bool HasPrevious { get; private set; }
    public bool HasNext { get; private set; }
}