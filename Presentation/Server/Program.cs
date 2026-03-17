using Presentation.Server;
using Presentation.Utils;

namespace SLAIS.Presentation.Server;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Startup.ConfigureServices(builder);

        var app = builder.Build();

        app.Migrate();

        Startup.ConfigurePipeline(app);

        app.Run();
    }
}
