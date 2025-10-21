using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CollectApp.Models;

namespace CollectApp.Data;

public class CollectAppContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public CollectAppContext(DbContextOptions<CollectAppContext> options, IHttpContextAccessor? httpContextAccessor = null)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Collect> Collects { get; set; } = default!;
    public DbSet<Supplier> Suppliers { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Filial> Filials { get; set; } = default!;
    public DbSet<AuditLog> AuditLogs { get; set; } = default!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Sistema";
        var now = DateTime.UtcNow;
        var auditEntries = new List<AuditLog>();

        foreach (var entry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified))
        {
            var entityName = entry.Entity.GetType().Name;
            var primaryKey = entry.Properties
                .FirstOrDefault(p => p.Metadata.IsPrimaryKey())?
                .CurrentValue?.ToString();

            foreach (var prop in entry.Properties)
            {
                if (prop.Metadata.Name is "UpdatedAt" or "LastModified" or "LastUpdatedBy")
                {
                    continue;
                }

                if (prop.IsModified && !Equals(prop.OriginalValue, prop.CurrentValue))
                {
                    auditEntries.Add(new AuditLog
                    {
                        EntityName = entityName,
                        EntityId = int.TryParse(primaryKey, out var id) ? id : 0,
                        Field = prop.Metadata.Name,
                        OldValue = prop.OriginalValue?.ToString() ?? "",
                        NewValue = prop.CurrentValue?.ToString() ?? "",
                        UserName = userName,
                        ChangedAt = now
                    });
                }
            }
        }

        if (auditEntries.Count != 0)
        {
            AuditLogs.AddRange(auditEntries);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }


}