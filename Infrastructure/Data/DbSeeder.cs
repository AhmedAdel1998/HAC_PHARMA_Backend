using Microsoft.AspNetCore.Identity;
using HAC_Pharma.Domain.Entities;

namespace HAC_Pharma.Infrastructure.Data;

/// <summary>
/// Database seeder for initial data including admin user
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        // Seed roles
        await SeedRolesAsync(roleManager);

        // Seed admin user
        await SeedAdminUserAsync(userManager);
    }

    private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        string[] roles = { "Admin", "Editor", "Viewer" };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new ApplicationRole
                {
                    Name = roleName,
                    Description = $"{roleName} role for HAC Pharma CMS"
                };
                await roleManager.CreateAsync(role);
                Console.WriteLine($"✓ Created role: {roleName}");
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
        // Admin user configuration - CHANGE THESE VALUES!
        const string adminEmail = "admin@hacpharma.com";
        const string adminPassword = "Admin@123456"; // Change this in production!
        const string adminFirstName = "System";
        const string adminLastName = "Administrator";

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin != null)
        {
            Console.WriteLine($"✓ Admin user exists: {adminEmail}. Resetting password...");
            var token = await userManager.GeneratePasswordResetTokenAsync(existingAdmin);
            var resetResult = await userManager.ResetPasswordAsync(existingAdmin, token, adminPassword);
            if (resetResult.Succeeded)
            {
                // Ensure IsActive is true and Account is Unlocked
                existingAdmin.IsActive = true;
                existingAdmin.LockoutEnd = null;
                await userManager.UpdateAsync(existingAdmin);
                Console.WriteLine($"✓ Password reset to: {adminPassword}");
            }
            else
            {
                Console.WriteLine("✗ Failed to reset password.");
                foreach(var err in resetResult.Errors) Console.WriteLine($" - {err.Description}");
            }
            return;
        }

        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = adminFirstName,
            LastName = adminLastName,
            EmailConfirmed = true,
            IsActive = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
            Console.WriteLine($"✓ Created admin user: {adminEmail}");
            Console.WriteLine($"  Password: {adminPassword}");
            Console.WriteLine("  ⚠️  IMPORTANT: Change this password immediately in production!");
        }
        else
        {
            Console.WriteLine($"✗ Failed to create admin user:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"  - {error.Description}");
            }
        }
    }
}
