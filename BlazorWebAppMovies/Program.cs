using BlazorWebAppMovies.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlazorWebAppMovies.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure PostgreSQL database connection
var connectionString = builder.Configuration.GetConnectionString("BlazorWebAppMoviesContext") 
    ?? throw new InvalidOperationException("Connection string 'BlazorWebAppMoviesContext' not found.");

// Log connection string for debugging (mask password)
var maskedConnectionString = connectionString.Contains("Password=") 
    ? System.Text.RegularExpressions.Regex.Replace(connectionString, @"Password=([^;]+)", "Password=***")
    : connectionString;
Console.WriteLine($"=== DATABASE CONFIGURATION ===");
Console.WriteLine($"Connection String: {maskedConnectionString}");
Console.WriteLine($"================================");

builder.Services.AddDbContextFactory<BlazorWebAppMoviesContext>(options =>
    options.UseNpgsql(connectionString));

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
                logger.LogInformation("Attempting to connect to PostgreSQL database and apply migrations...");
                
                // Apply migrations
                logger.LogInformation("Applying database migrations...");
                context.Database.Migrate();
                
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
