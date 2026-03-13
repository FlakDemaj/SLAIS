using Infrastructure.Persistence;

namespace Presentation.Utils;

public static class WebApplicationExtensions
{
    public static void Migrate(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<MigrationManager>();
        migrator.Migrate();
    }
}