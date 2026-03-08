// API/Startup/Startup.cs

using SAIS.Domain.Commom.Logger;
using SAIS.Infrastructure.DependencyInjection;
using SAIS.Infrastructure.InternalServices.Logging;
namespace YourProject.API.Startup;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        ConfigureSentry(builder);
        ConfigurePresentation(builder.Services);
        ConfigureLayers(builder.Services);
    }

    public static void ConfigurePipeline(WebApplication app)
    {
        ConfigureMiddleware(app);
        ConfigureStaticLogger(app);
    }

    private static void ConfigureSentry(WebApplicationBuilder builder)
    {
        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = builder.Configuration["Sentry:Dsn"];
            options.Debug = false;
            options.TracesSampleRate = 1.0;
            options.MinimumEventLevel = LogLevel.Warning;
        });
    }

    private static void ConfigurePresentation(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
    }

    private static void ConfigureLayers(IServiceCollection services)
    {
        services.AddInfrastructure();
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
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }

    private static void ConfigureStaticLogger(WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ISAISLogger>();

        StaticLogger.Initialize(logger);
    }
}