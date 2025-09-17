namespace CollectApp.ViewModels
{
    public class PagedResultViewModel<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
    }
}