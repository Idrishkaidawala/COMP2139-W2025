using Microsoft.AspNetCore.Identity;
using SmartInventoryManagement.Models; // Add this line to reference the Product class
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SmartInventoryManagement.Data
{
    public static class DataSeeding
    {
        public static void SeedDatabase(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and accessories" },
                    new Category { Id = 2, Name = "Clothing", Description = "Apparel and fashion items" },
                    new Category { Id = 3, Name = "Food", Description = "Food and beverages" }
                );
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Id = 1, Name = "Product 1", Price = 10, QuantityInStock = 100, LowStockThreshold = 10, CategoryId = 1 },
                    new Product { Id = 2, Name = "Product 2", Price = 20, QuantityInStock = 200, LowStockThreshold = 20, CategoryId = 2 }
                );
                context.SaveChanges();
            }

            // Reset the sequence for Products table
            context.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"Products\"', 'Id'), (SELECT MAX(\"Id\") FROM \"Products\"), true);");
        }

        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create roles if they don't exist
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create admin user if it doesn't exist
            var adminEmail = "admin@inventory.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    Address = "System Address",
                    PhoneNumber = "1234567890",
                    PreferredCategories = new List<int>(),
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Create a regular user if it doesn't exist
            var userEmail = "user@inventory.com";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FullName = "Regular User",
                    Address = "User Address",
                    PhoneNumber = "0987654321",
                    PreferredCategories = new List<int>(),
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "User123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}