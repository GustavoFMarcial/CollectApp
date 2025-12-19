using CollectApp.Models;

namespace CollectApp.Dto;

public class CollectPerStatusDto
{
    public CollectStatus Status { get; set; }
    public int Total { get; set; }
}