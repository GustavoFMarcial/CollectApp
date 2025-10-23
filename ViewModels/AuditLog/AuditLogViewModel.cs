namespace CollectApp.ViewModels;

public class AuditLogViewModel
{
    public string Field { get; set; } = string.Empty;
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
}