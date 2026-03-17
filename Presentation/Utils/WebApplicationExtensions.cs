using Infrastructure.Persistence;

namespace Presentation.Utils;

public static class WebApplicationExtensions
{
    public static void Migrate(this WebApplication webApplication)
    {
        using IServiceScope scope = webApplication.Services.CreateScope();
        MigrationManager migrator = scope.ServiceProvider.GetRequiredService<MigrationManager>();
        migrator.Migrate();
    }
}
