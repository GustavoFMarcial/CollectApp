namespace CollectApp.ViewModels;

public class FilterRequestInput
{
    public int PageNum { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string Input { get; set; } = string.Empty;
}