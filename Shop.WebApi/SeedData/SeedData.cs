using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Enums;

namespace Shop.WebAPI.SeedData;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ShopApplicationContext>();

            try
            {
                // Применение миграций
                await context.Database.MigrateAsync();
                //await ApplyPendingMigrationsAsync(context);
                await EnsureRolesExistAsync(roleManager);
                await EnsureUsersAndRolesExistAsync(userManager, roleManager);
                await EnsureCategoriesExistAsync(context);
                await EnsureBrandsExistAsync(context);
                await EnsureProductsExistAsync(context);
                await EnsureAddressesExistAsync(context);
                await EnsureCommentsExistAsync(context);
                await EnsureOrdersExistAsync(context);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during initialization.");
                throw;
            }
        }
    }
    
    private static async Task EnsureRolesExistAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    private static async Task EnsureUsersAndRolesExistAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var adminUser = await userManager.FindByEmailAsync("admin@example.com");
        if (adminUser == null)
        {
            adminUser = new IdentityUser { UserName = "admin", Email = "admin@example.com" };
            await userManager.CreateAsync(adminUser, "Admin@123");
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        var normalUser = await userManager.FindByEmailAsync("user@example.com");
        if (normalUser == null)
        {
            normalUser = new IdentityUser { UserName = "user", Email = "user@example.com" };
            await userManager.CreateAsync(normalUser, "User@123");
        }

        if (!await userManager.IsInRoleAsync(normalUser, "User"))
        {
            await userManager.AddToRoleAsync(normalUser, "User");
        }
    }
    private static async Task EnsureCategoriesExistAsync(ShopApplicationContext context)
    {
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                
                new Category
                {
                    Id = 1,
                    Name = "Women's shoes"
                },
                new Category
                {
                    Id = 2,
                    Name = "Men's shoes"
                },
                new Category
                {
                    Id = 3,
                    Name = "Children's shoes"
                }
            );
            await context.SaveChangesAsync();
        }
    }
    private static async Task EnsureBrandsExistAsync(ShopApplicationContext context)
    {
        if (!context.Brands.Any())
        {
            context.Brands.AddRange(
                new Brand { Name = "Nike" },
                new Brand { Name = "Adidas" },
                new Brand { Name = "Puma" }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task EnsureProductsExistAsync(ShopApplicationContext context)
    {
        if (!context.Products.Any())
        {
            var products = new List<Product>();

            var sizes = new int[] { 38, 39, 40, 41, 42, 43 }; // Пример доступных размеров
            var colors = new[] { "Red", "Blue", "Green", "Black", "White" }; // Пример цветов
            //var brands = new[] { "Nike", "Adidas", "Puma", "Reebok" }; // Пример брендов
            var materials = new[] { "Leather", "Synthetic", "Mesh" }; // Пример материалов

            for (int i = 1; i <= 25; i++)
            {
                products.Add(new Product
                {
                    Id = i,
                    Name = $"Shoes {i}",
                    Description = $"Description for Product {i}",
                    Price = 10.00m + i, // Логика расчета цены
                    StockQuantity = 100 - (i % 10), // Пример логики для количества на складе
                    ImageUrl = $"{i}.jpeg",
                    CategoryId = 1, // Пример логики для категории

                    // Новые свойства
                    Sizes = sizes, // Размер
                    Color = colors[i % colors.Length], // Цвет
                    BrandId = 1, // Бренд
                    Material = materials[i % materials.Length], // Материал
                    IsAvailable = true // Наличие
                });
            }

            for (int i = 26; i <= 50; i++)
            {
                products.Add(new Product
                {
                    Id = i,
                    Name = $"Shoes {i}",
                    Description = $"Description for Product {i}",
                    Price = 10.00m + i, // Логика расчета цены
                    StockQuantity = 100 - (i % 10), // Пример логики для количества на складе
                    ImageUrl = $"{i}.jpeg",
                    CategoryId = 2, // Пример логики для категории

                    // Новые свойства
                    Sizes = sizes, // Размер
                    Color = colors[i % colors.Length], // Цвет
                    BrandId = 2, // Бренд
                    Material = materials[i % materials.Length], // Материал
                    IsAvailable = true // Наличие
                });
            }

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
    private static async Task EnsureAddressesExistAsync(ShopApplicationContext context)
    {
        if (!context.Addresses.Any())
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com");
            context.Addresses.AddRange(
                new Address
                {
                    Id = 1,
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "CA",
                    ZipCode = "12345",
                    UserId = user.Id
                },
                new Address
                {
                    Id = 2,
                    Street = "456 Elm Ave",
                    City = "Othertown",
                    State = "NY",
                    ZipCode = "67890",
                    UserId = user.Id
                }
            );
            await context.SaveChangesAsync();
        }
    }
    private static async Task EnsureCommentsExistAsync(ShopApplicationContext context)
    {
        if (!context.Comments.Any())
        {
            var products = await context.Products.ToListAsync();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com");

            context.Comments.AddRange(
                new Comment
                {
                    Id = 1,
                    Text = "Great product! Fast shipping!",
                    CreatedAt = DateTime.UtcNow,
                    ProductId = products[0].Id,
                    UserId = user.Id
                },
                new Comment
                {
                    Id = 2,
                    Text = "Good price for quality.",   
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    ProductId = products[^1].Id, // Последний продукт
                    UserId = user.Id
                }
            );
            await context.SaveChangesAsync();
        }
    }
    private static async Task EnsureOrdersExistAsync(ShopApplicationContext context)
    {
        if (!context.Orders.Any())
        {
            var products = await context.Products.ToListAsync();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com");

            if (products.Count >= 2 && user != null)
            {
                var orderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = products[0].Id, Quantity = 2, UnitPrice = products[0].Price },
                    new OrderItem { ProductId = products[^1].Id, Quantity = 1, UnitPrice = products[^1].Price }
                };

                context.Orders.Add(
                    new Order
                    {
                        Id = 1,
                        UserId = user.Id,
                        OrderDate = DateTime.UtcNow,
                        Status = OrderStatus.Processed,
                        TotalAmount = orderItems.Sum(oi => oi.Quantity * oi.UnitPrice),
                        OrderItems = orderItems
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
