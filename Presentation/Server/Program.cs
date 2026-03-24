using Infrastructure.Persistence.Seeding;

using Presentation.Server;
using Presentation.Utils;

namespace SLAIS.Presentation.Server;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Startup.ConfigureServices(builder);

        var app = builder.Build();

        if (!app.Environment.IsEnvironment("Testing"))
        {
            app.Migrate();

            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<ServerUserSeeder>();
                await seeder.SeedAsync();
            }
        }

        Startup.ConfigurePipeline(app);

        await app.RunAsync();

    }
}
