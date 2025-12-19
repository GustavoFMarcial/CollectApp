using CollectApp.Models;

namespace CollectApp.Dtos;

public class CollectPerStatusDto
{
    public CollectStatus Status { get; set; }
    public int Total { get; set; }
}