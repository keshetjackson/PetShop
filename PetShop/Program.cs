using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using PetShop.Data;
using PetShop.Repositories;

// Set up NLog logger
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    // Create a WebApplication object
    var builder = WebApplication.CreateBuilder(args);

    // Configure NLog for dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container
    builder.Services.AddControllersWithViews();

    // Configure the DbContext
    var connectionString = builder.Configuration.GetConnectionString("MyConnection");
    builder.Services.AddDbContext<MyDbContext>(options => options.UseLazyLoadingProxies().UseSqlite(connectionString));

    // Add the ShopRepository to the container
    builder.Services.AddScoped<IShopRepository, ShopRepository>();

    // Build the application and ensure the database is created
    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        dbContext?.Database.EnsureDeleted();
        dbContext?.Database.EnsureCreated();
    }

    // Configure the HTTP request pipeline
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    }
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

    // Run the application
    app.Run();
}
catch (Exception e)
{
    // Log any exceptions using NLog
    logger.Error(e, "Stopped program because of exception");
    throw;
}
finally
{
    // Shut down the NLog logger
    NLog.LogManager.Shutdown();
}
