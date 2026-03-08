using YourProject.API.Startup;

namespace SAIS.Presentation.Server;

public static class Program
{
    public static void Main(String[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        Startup.ConfigureServices(builder);
        
        var app = builder.Build();
        
        Startup.ConfigurePipeline(app);
                
        app.Run();
    }
}