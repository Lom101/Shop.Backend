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
                    Name = "Electronics"
                },
                new Category
                {
                    Id = 2,
                    Name = "Books"
                }
            );
            await context.SaveChangesAsync();
        }
    }
    private static async Task EnsureProductsExistAsync(ShopApplicationContext context)
    {
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "Description for Product 1",
                    Price = 10.99m,
                    StockQuantity = 100,
                    ImageUrl = "https://example.com/product1.jpg",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "Description for Product 2",
                    Price = 19.99m,
                    StockQuantity = 50,
                    ImageUrl = "https://example.com/product2.jpg",
                    CategoryId = 2
                }
            );
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
    
    // public static async Task ApplyPendingMigrationsAsync(DbContext  _context)
    // {
    //     // Получаем список примененных миграций
    //     var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
    //     
    //     // Получаем список ожидающих миграций
    //     var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
    //     
    //     // Если есть ожидающие миграции, применяем их
    //     if (pendingMigrations.Any())
    //     {
    //         Console.WriteLine("Applying pending migrations...");
    //         await _context.Database.MigrateAsync();
    //         Console.WriteLine("Migrations applied.");
    //     }
    //     else
    //     {
    //         Console.WriteLine("No pending migrations to apply.");
    //     }
    // }   
    
    // public static async Task Initialize(IServiceProvider serviceProvider)
    // {
    //     using (var scope = serviceProvider.CreateScope())
    //     {
    //         var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //         var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    //         var context = scope.ServiceProvider.GetRequiredService<ShopApplicationContext>();
    //         try
    //         {
    //             context.Database.Migrate(); // Применение миграций
    //         }
    //         catch (Exception ex)
    //         {
    //             // Логирование ошибок при применении миграций
    //             var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    //             logger.LogError(ex, "An error occurred while migrating the database.");
    //         }
    //
    //         
    //         // Create roles
    //         string[] roles = { "Admin", "User" };
    //         foreach (var role in roles)
    //         {
    //             if (!await roleManager.RoleExistsAsync(role))
    //             {
    //                 await roleManager.CreateAsync(new IdentityRole(role));
    //             }
    //         }
    //
    //         // Create admin user
    //         var adminUser = await userManager.FindByEmailAsync("admin@example.com");
    //         if (adminUser == null)
    //         {
    //             adminUser = new IdentityUser { UserName = "admin", Email = "admin@example.com" };
    //             await userManager.CreateAsync(adminUser, "Admin@123");
    //         }
    //
    //         // Add user to Admin role
    //         if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
    //         {
    //             await userManager.AddToRoleAsync(adminUser, "Admin");
    //         }
    //         
    //         // Create normal user
    //         var normalUser = await userManager.FindByEmailAsync("user@example.com");
    //         if (normalUser == null)
    //         {
    //             normalUser = new IdentityUser { UserName = "user", Email = "user@example.com" };
    //             await userManager.CreateAsync(normalUser, "User@123");
    //         }
    //
    //         // Add user to User role
    //         if (!await userManager.IsInRoleAsync(normalUser, "User"))
    //         {
    //             await userManager.AddToRoleAsync(normalUser, "User");
    //         }
    //         
    //         // Проверка наличия данных в таблице Category
    //         if (!context.Categories.Any())
    //         {
    //             // Если данных нет, добавляем их
    //             context.Categories.AddRange(
    //                 new Category
    //                 {
    //                     Id = 1,
    //                     Name = "Electronics"
    //                 },
    //                 new Category
    //                 {
    //                     Id = 2,
    //                     Name = "Books"
    //                 }
    //             );
    //             await context.SaveChangesAsync();
    //         }
    //         
    //         // добавление в Product и Проверка наличия данных в таблице Product
    //         if (!context.Products.Any())
    //         {
    //             // Если данных нет, добавляем их
    //             context.Products.AddRange(
    //                 new Product
    //                 {
    //                     Name = "Product 1",
    //                     Description = "Description for Product 1",
    //                     Price = 10.99m,
    //                     StockQuantity = 100,
    //                     ImageUrl = "https://example.com/product1.jpg",
    //                     CategoryId = 1 // Предполагается, что категория с Id = 1 уже существует
    //                 },
    //                 new Product
    //                 {
    //                     Name = "Product 2",
    //                     Description = "Description for Product 2",
    //                     Price = 19.99m,
    //                     StockQuantity = 50,
    //                     ImageUrl = "https://example.com/product2.jpg",
    //                     CategoryId = 2 // Предполагается, что категория с Id = 2 уже существует
    //                 }
    //             );
    //
    //             await context.SaveChangesAsync();
    //         }
    //         
    //         // Добавление данных для таблицы Address
    //         if (!context.Addresses.Any())
    //         {
    //             context.Addresses.AddRange(
    //                 new Address
    //                 {
    //                     Street = "123 Main St",
    //                     City = "Anytown",
    //                     State = "CA",
    //                     ZipCode = "12345",
    //                     UserId = normalUser.Id
    //                 },
    //                 new Address
    //                 {
    //                     Street = "456 Elm Ave",
    //                     City = "Othertown",
    //                     State = "NY",
    //                     ZipCode = "67890",
    //                     UserId = normalUser.Id
    //                 }
    //             );
    //             await context.SaveChangesAsync();
    //         }
    //         
    //         // Добавление данных для таблицы Comment
    //         if (!context.Comments.Any())
    //         {
    //             context.Comments.AddRange(
    //                 new Comment
    //                 {
    //                     Id = 1,
    //                     Text = "Great product! Fast shipping!",
    //                     CreatedAt = DateTime.Now,
    //                     ProductId = 1,
    //                     UserId = normalUser.Id
    //                 },
    //                 new Comment
    //                 {
    //                     Id = 2,
    //                     Text = "Good price for quality.",
    //                     CreatedAt = DateTime.Now.AddDays(-7),
    //                     ProductId = 2,
    //                     UserId = normalUser.Id
    //                 }
    //             );
    //             await context.SaveChangesAsync();
    //         }
    //         
    //         // Добавление данных для таблицы Order
    //         if (!context.Orders.Any())
    //         {
    //             var orderItems = new List<OrderItem>
    //             {
    //                 new OrderItem { ProductId = 1, Quantity = 2, Price = 10.99m },
    //                 new OrderItem { ProductId = 2, Quantity = 1, Price = 19.99m }
    //             };
    //
    //             context.Orders.AddRange(
    //                 new Order
    //                 {
    //                     Id = 1,
    //                     UserId = normalUser.Id,
    //                     OrderDate = DateTime.Now,
    //                     Status = OrderStatus.Processed,
    //                     TotalAmount = 39.98m,
    //                     OrderItems = orderItems
    //                 }
    //             );
    //
    //             await context.SaveChangesAsync();
    //         }
    //     }
    // }
}
