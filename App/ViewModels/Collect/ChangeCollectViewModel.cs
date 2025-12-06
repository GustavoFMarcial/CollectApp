using System.ComponentModel.DataAnnotations;
using CollectApp.Models;


namespace CollectApp.ViewModels;

public class ChangeCollectViewModel
{
    [Required]
    public int Id { get; set; }
    public CollectStatus Status { get; set; }
    public bool ToOpen { get; set; } = false;
    public string UserId { get; set; } = string.Empty;
    public bool CanChangeCollectStatus { get; set; }
    public bool CanEditOpenOrDeleteCollect { get; set; }
}