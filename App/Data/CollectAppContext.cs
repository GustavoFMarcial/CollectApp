using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CollectApp.Models;

namespace CollectApp.Data;

public class CollectAppContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;
    public DbSet<Collect> Collects { get; set; } = default!;
    public DbSet<Supplier> Suppliers { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Filial> Filials { get; set; } = default!;
    public DbSet<AuditLog> AuditLogs { get; set; } = default!;

    public CollectAppContext(DbContextOptions<CollectAppContext> options, IHttpContextAccessor? httpContextAccessor = null)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        string userName = _httpContextAccessor?.HttpContext?.User?.FindFirst("FullName")?.Value ?? "Sistema";
        DateTime now = DateTime.UtcNow;
        List<AuditLog> auditEntries = new List<AuditLog>();

        foreach (var entry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified))
        {
            var entityName = entry.Entity.GetType().Name;
            var primaryKey = entry.Properties
                .FirstOrDefault(p => p.Metadata.IsPrimaryKey())?
                .CurrentValue?.ToString();

            foreach (var prop in entry.Properties)
            {
                if (prop.Metadata.Name is "UpdatedAt" or "LastModified" or "LastUpdatedBy" or "ConcurrencyStamp" or "SecurityStamp")
                {
                    continue;
                }

                if (prop.IsModified && !Equals(prop.OriginalValue, prop.CurrentValue))
                {
                    auditEntries.Add(new AuditLog
                    {
                        EntityName = entityName,
                        EntityId = primaryKey ?? "",
                        Field = prop.Metadata.Name,
                        OldValue = prop.OriginalValue?.ToString() ?? "",
                        NewValue = prop.CurrentValue?.ToString() ?? "",
                        UserName = userName,
                        ChangedAt = now,
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