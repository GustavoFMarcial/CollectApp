using CollectApp.Authorization.Requirements;
using CollectApp.Data;
using CollectApp.Models;
using CollectApp.Repositories;
using CollectApp.Services;
using CollectApp.Authorization.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using CollectApp.Factories;
using CollectApp.Middlewares;
using CollectApp.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICollectService, CollectService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFilialService, FilialService>();
builder.Services.AddScoped<ICollectRepository, CollectRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IFilialRepository, FilialRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
builder.Services.AddScoped<IAuthorizationHandler, MustBeCollectOwnerHandler>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var connectionString = builder.Configuration.GetConnectionString("CollectAppContext") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<CollectAppContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<CollectAppContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanChangeCollectStatus", policy =>
        policy.RequireRole("Gestor", "Admin"));

    options.AddPolicy("CanInsert", policy =>
        policy.RequireRole("Comprador", "Admin"));

    options.AddPolicy("CanEditAndDeleteCollect", policy =>
        policy.RequireRole("Comprador", "Admin"));

    options.AddPolicy("MustBeCollectOwner", policy =>
        policy.Requirements.Add(new MustBeCollectOwnerRequirement()));

    options.AddPolicy("CanCreateAndEditUsers", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = false;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = new[] { "Admin", "Gestor", "Comprador" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var adminUserName = "admin";
    var admin = await userManager.FindByNameAsync(adminUserName);
    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = adminUserName,
            Email = "admin@admin.com",
            FullName = "Admin",
            Role = "Admin",
            Status = UserStatus.Ativo,
            CreatedAt = DateTime.Now,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, "Gpe.1@345");
        if (!result.Succeeded)
        {
            throw new Exception("Falha ao criar usuário admin: " +
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

#if DEBUG
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CollectAppContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Verificando necessidade de seed...");
        
        await context.Database.MigrateAsync();
        
        await DatabaseSeeder.SeedCollects(context, quantidade: 300);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao popular banco de dados com seed");
    }
}
#endif

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Shared/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<CheckLockoutMiddleware>();
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Collect}/{action=ListCollects}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();