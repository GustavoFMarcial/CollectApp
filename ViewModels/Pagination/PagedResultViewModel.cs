namespace CollectApp.ViewModels
{
    public class PagedResultViewModel<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalPages { get; set; }
        public int PageNum { get; set; }
        public CollectFilterViewModel Filters { get; set; } = new();
    }
}