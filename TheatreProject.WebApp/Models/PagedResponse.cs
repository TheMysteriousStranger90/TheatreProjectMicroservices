namespace TheatreProject.WebApp.Models;

public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;

    public PagedResponse()
    {
        Data = new List<T>();
    }

    public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    }

    public string GetPaginationInfo()
    {
        return $"Page {PageNumber} of {TotalPages} ({TotalRecords} total records)";
    }

    public int[] GetPaginationRange(int maxPagesToShow = 5)
    {
        var start = Math.Max(1, PageNumber - maxPagesToShow / 2);
        var end = Math.Min(TotalPages, start + maxPagesToShow - 1);
        
        if (end - start + 1 < maxPagesToShow)
        {
            start = Math.Max(1, end - maxPagesToShow + 1);
        }

        return Enumerable.Range(start, end - start + 1).ToArray();
    }
}