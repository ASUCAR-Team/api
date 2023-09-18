using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Extensions;

public static class ApplicationSeedDatabaseExtensions
{
    public static async void SeedDatabase(WebApplication webApplication)
    {
        var scope = webApplication.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            await context.Database.MigrateAsync();
            await Seed.SeedRolesAsync(context);
            await Seed.SeedSkillTypesAsync(context);
            await Seed.SeedAdminAsync(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, " A problem occurred during seeding ");
        }
    }
}