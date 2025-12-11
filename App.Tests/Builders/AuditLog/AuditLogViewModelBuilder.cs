using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class AuditLogViewModelBuilder
{
    private string _field = "Name";
    private string _oldValue = "Valor Antigo";
    private string _newValue = "Valor Novo";
    private string _userName = "usuario.teste";
    private DateTime _changedAt = DateTime.Now;

    public AuditLogViewModelBuilder FromAuditLog(AuditLog a)
    {
        _field = a.Field;
        _oldValue = a.OldValue;
        _newValue = a.NewValue;
        _userName = a.UserName;
        _changedAt = a.ChangedAt;
        return this;
    }

    public AuditLogViewModel Build()
    {
        return new AuditLogViewModel
        {
            Field = _field,
            OldValue = _oldValue,
            NewValue = _newValue,
            UserName = _userName,
            ChangedAt = TimeZoneInfo.ConvertTimeFromUtc(_changedAt, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString("dd/MM/yyyy HH:mm"),
        };
    }
}