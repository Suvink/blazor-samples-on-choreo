using BlazorWebAppMovies.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlazorWebAppMovies.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure database provider based on environment variable or configuration
var databaseProvider = builder.Configuration["DATABASE_PROVIDER"] ?? "SqlServer";
var connectionString = builder.Configuration.GetConnectionString("BlazorWebAppMoviesContext") 
    ?? throw new InvalidOperationException("Connection string 'BlazorWebAppMoviesContext' not found.");

builder.Services.AddDbContextFactory<BlazorWebAppMoviesContext>(options =>
{
    switch (databaseProvider.ToLower())
    {
        case "postgresql":
        case "postgres":
            options.UseNpgsql(connectionString);
            break;
        case "sqlserver":
        default:
            options.UseSqlServer(connectionString);
            break;
    }
});

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Ensure database is created and migrations are applied
var retryCount = 0;
var maxRetries = 10;
var retryDelay = TimeSpan.FromSeconds(3);

while (retryCount < maxRetries)
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            var contextFactory = services.GetRequiredService<IDbContextFactory<BlazorWebAppMoviesContext>>();
            
            using (var context = contextFactory.CreateDbContext())
            {
                logger.LogInformation("Attempting to connect to database and apply schema...");
                
                // Use EnsureCreated for PostgreSQL, Migrate for SQL Server
                if (databaseProvider.ToLower() is "postgresql" or "postgres")
                {
                    logger.LogInformation("Using EnsureCreated for PostgreSQL...");
                    context.Database.EnsureCreated();
                }
                else
                {
                    logger.LogInformation("Applying migrations for SQL Server...");
                    context.Database.Migrate();
                }
                
                logger.LogInformation("Database schema ready.");
            }
            
            // Seed data after migrations are complete
            logger.LogInformation("Seeding database...");
            SeedData.Initialize(services);
            logger.LogInformation("Database seeded successfully.");
        }
        break; // Success, exit the retry loop
    }
    catch (Exception ex)
    {
        retryCount++;
        if (retryCount >= maxRetries)
        {
            Console.WriteLine($"Failed to initialize database after {maxRetries} attempts. Error: {ex.Message}");
            throw;
        }
        Console.WriteLine($"Database initialization attempt {retryCount} failed. Retrying in {retryDelay.TotalSeconds} seconds... Error: {ex.Message}");
        Thread.Sleep(retryDelay);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
