using CollectApp.Helpers;
using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class AuditLogViewModelBuilder
{
    private string _field = string.Empty;
    private string _oldValue = string.Empty;
    private string _newValue = string.Empty;
    private string _userName = string.Empty;
    private DateTime _changedAt;

    public AuditLogViewModelBuilder FromAuditLog(AuditLog a)
    {
        a.Field = LogFieldTranslations.Translate(a.EntityName, a.Field);

        if (a.OldValue == "PendenteAprovar" || a.OldValue == "PendenteColetar")
        {
            a.OldValue = LogStatusValue.LogStatusValueSpaceBetween(a.OldValue);
        }

        if (a.NewValue == "PendenteAprovar" || a.NewValue == "PendenteColetar")
        {
            a.NewValue = LogStatusValue.LogStatusValueSpaceBetween(a.NewValue);
        }

        _field = a.Field;
        _oldValue = a.OldValue;
        _newValue = a.NewValue;
        _userName = a.UserName;
        _changedAt = DateTime.SpecifyKind(a.ChangedAt, DateTimeKind.Utc);
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