namespace CollectApp.ViewModels;

public class PagedResultViewModel<T, TFilter> where TFilter : class, new()
{
    public List<T> Items { get; set; } = new();
    public int TotalPages { get; set; }
    public int PageNum { get; set; }
    public TFilter Filters { get; set; } = new();
}