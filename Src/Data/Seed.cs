using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class Seed
{
    public static async Task SeedRolesAsync(DataContext dataContext)
    {
        if (dataContext is null)
            throw new ArgumentNullException(nameof(dataContext));
        
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        if (await dataContext.Roles.AnyAsync())
            return;
        
        var rolesData = await File.ReadAllTextAsync("Src/Data/Seeds/RolesData.json");
        var rolesList = JsonSerializer.Deserialize<List<Role>>(rolesData, options);
        
        if (rolesList is null)
            return;
        
        await dataContext.Roles.AddRangeAsync(rolesList);
        await dataContext.SaveChangesAsync();
    }
    
    public static async Task SeedSkillTypesAsync(DataContext dataContext)
    {
        if (dataContext is null)
            throw new ArgumentNullException(nameof(dataContext));
        
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        if (await dataContext.SkillTypes.AnyAsync())
            return;
        
        var skillTypesData = await File.ReadAllTextAsync("Src/Data/Seeds/SkillTypesData.json");
        var skillTypeList = JsonSerializer.Deserialize<List<SkillType>>(skillTypesData, options);
        
        if (skillTypeList is null)
            return;
        
        await dataContext.SkillTypes.AddRangeAsync(skillTypeList);
        await dataContext.SaveChangesAsync();
    }

    public static async Task SeedAdminAsync(DataContext dataContext)
    {
        if (dataContext == null)
            throw new ArgumentNullException(nameof(dataContext));
        
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        if (await dataContext.Users.AnyAsync(x => x.Role.Name == "admin"))
            return;
        
        var adminData = await File.ReadAllTextAsync("Src/Data/Seeds/AdminData.json");
        var admin = JsonSerializer.Deserialize<User>(adminData, options);

        if (admin is null)
            return;
        
        using var hmac = new HMACSHA512();
        admin.PasswordHash = hmac.ComputeHash("password"u8.ToArray());
        admin.PasswordSalt = hmac.Key;
        admin.Role = await dataContext.Roles.FirstAsync(x => x.Name == "admin");

        await dataContext.Users.AddAsync(admin);
        await dataContext.SaveChangesAsync();
    }
}