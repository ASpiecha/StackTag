using StackTag.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StackTag;
using System.Reflection;
using Microsoft.OpenApi.Models;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

MigrateDatabase(app);

if (app.Environment.IsDevelopment())
{
    ConfigureDevelopmentServices(app);
}
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<DataContext>((sp, options) =>
    {
        options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        options.UseSqlServer(connectionString/*, options => options.EnableRetryOnFailure()*/);
    });

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    services.AddControllers();
    services.AddScoped<IDataContext>(p => p.GetRequiredService<DataContext>());
    services.AddEndpointsApiExplorer();
    services.AddHttpContextAccessor();
    services.AddTransient<IStackOverflowAPI, StackOverflowAPI>();
    services.AddTransient<ITagAnalyzer, TagAnalyzer>();
    services.AddTransient<ILoggerAdapter<TagAnalyzer>, LoggerAdapter<TagAnalyzer>>();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "StackTagAPI", Version = "v1" });
    });

    services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://localhost:8080")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });
}

void MigrateDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Migrating database...");

        var db = services.GetRequiredService<DataContext>().Database;
        int maxRetries = 10;
        int currentRetry = 0;
        bool migrated = false;

        while (!migrated && currentRetry < maxRetries)
        {
            try
            {
                db.Migrate();
                logger.LogInformation("Database migrated successfully.");
                migrated = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while applying migrations - retry {currentRetry}");
                currentRetry++;
                Task.Delay(2000).Wait();
            }
        }

        if (!migrated)
        {
            logger.LogError("Failed to migrate database after multiple attempts.");
            throw new Exception("Database migration failed.");
        }
   }
}

void ConfigureDevelopmentServices(WebApplication app)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StackTagAPI v1");
    });

    Task.Run(() => OpenSwaggerInBrowser(app.Services.GetRequiredService<ILogger<Program>>()));
}

void OpenSwaggerInBrowser(ILogger<Program> logger)
{
    Task.Delay(5000).Wait();
    var containerPort = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';').FirstOrDefault()?.Split(':').LastOrDefault();
    if (containerPort != null)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "cmd",
            Arguments = $"/c start http://localhost:{containerPort}/swagger",
            CreateNoWindow = true
        });
    }
    else
    {
        logger.LogError("Failed to retrieve the container port.");
    }
}

public partial class Program { }