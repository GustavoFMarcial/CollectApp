using CollectApp.Models;

namespace CollectApp.Tests.Builders;

public class AuditLogBuilder
{
    private int _id = 1;
    private string _entityName = "Supplier";
    private string _entityId = "1";
    private string _field = "Name";
    private string _oldValue = "Valor Antigo";
    private string _newValue = "Valor Novo";
    private string _userName = "usuario.teste";
    private DateTime _changedAt = DateTime.UtcNow;

    public AuditLogBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public AuditLogBuilder WithEntityName(string entityName)
    {
        _entityName = entityName;
        return this;
    }

    public AuditLogBuilder WithEntityId(string entityId)
    {
        _entityId = entityId;
        return this;
    }

    public AuditLogBuilder WithField(string field)
    {
        _field = field;
        return this;
    }

    public AuditLogBuilder WithOldValue(string oldValue)
    {
        _oldValue = oldValue;
        return this;
    }

    public AuditLogBuilder WithNewValue(string newValue)
    {
        _newValue = newValue;
        return this;
    }

    public AuditLogBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public AuditLogBuilder WithChangedAt(DateTime changedAt)
    {
        _changedAt = changedAt;
        return this;
    }

    public AuditLog Build()
    {
        return new AuditLog
        {
            Id = _id,
            EntityName = _entityName,
            EntityId = _entityId,
            Field = _field,
            OldValue = _oldValue,
            NewValue = _newValue,
            UserName = _userName,
            ChangedAt = _changedAt
        };
    }
}