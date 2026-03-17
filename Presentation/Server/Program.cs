using Presentation.Server;
using Presentation.Utils;

namespace SLAIS.Presentation.Server;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        Startup.ConfigureServices(builder);

        WebApplication app = builder.Build();

        app.Migrate();

        Startup.ConfigurePipeline(app);

        app.Run();
    }
}
