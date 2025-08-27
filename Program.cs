using CollectApp.Data;
using CollectApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICollectService, CollectService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDbContext<CollectAppContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollectAppContext"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Collect}/{action=ListCollects}")
    .WithStaticAssets();


app.Run();
