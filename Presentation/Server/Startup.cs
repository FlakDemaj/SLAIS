using Infrastructure.Configurations;
using Presentation.Middlewares;
using SAIS.Infrastructure.DependencyInjection;

namespace Presentation.Server;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        EnableCors(builder.Services);
        
        ConfigureSentry(builder);
        ConfigureLogger(builder);
        ConfigureLayers(builder);
    }

    public static void ConfigurePipeline(WebApplication app)
    {
        ConfigureMiddleware(app);
    }
    
    private static void ConfigureLayers(WebApplicationBuilder builder)
    {
        AddPresentationLayer(builder.Services);
        AddInfrastructureLayer(builder);
    }
    
    private static void AddPresentationLayer(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
    }

    private static void AddInfrastructureLayer(WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructure();
        
        ConfigureOptions(builder);
    }
    
    private static void ConfigureSentry(WebApplicationBuilder builder)
    {
        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = !builder.Environment.IsProduction() ? "" : builder.Configuration["Sentry:Dsn"];
            options.Debug = false;
            options.TracesSampleRate = 0.10;
            options.MinimumEventLevel = LogLevel.Warning;
        });
    }
    
    private static void ConfigureLogger(WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders(); 
        builder.Logging.AddConsole(options => 
        {
            options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] "; 
            options.IncludeScopes = true;                          
        });
            
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
     }
    
    private static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseSentryTracing();
        EnableMiddlewares(app);
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }

    private static void EnableCors(IServiceCollection service)
    {
        service.AddCors(option =>
        {
            option.AddPolicy("AllowOrigin",
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }

    private static void EnableMiddlewares(IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionMiddleware>();
    }

    private static void ConfigureOptions(WebApplicationBuilder builder)
    {
        builder.Services.Configure<DatabaseOptions>(
            builder.Configuration.GetSection("Database"));
        builder.Services.Configure<TokenOptions>(
            builder.Configuration.GetSection("Access_Token"));
    }
}