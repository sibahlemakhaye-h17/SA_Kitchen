using Microsoft.AspNetCore.Identity;
using SA_Kitchen.Models;

namespace SA_Kitchen.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create roles
            string[] roleNames = ["Admin", "User"];
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user
            var adminEmail = "admin@sakitchen.co.za";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "SA_Kitchen",
                    LastName = "Admin",
                    CreatedAt = DateTime.Now,
                    IsAdmin = true,
                    Country = "South Africa",
                    Province = "Gauteng",
                    FavoriteCuisine = "Braai",
                    Bio = "Admin user for SA Kitchen", // Add this to avoid null
                    ProfilePictureUrl = null // Explicitly null
                };

                var result = await userManager.CreateAsync(user, "Admin@SA2024");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}