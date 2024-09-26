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

                await EnsureRolesExistAsync(roleManager);
                await EnsureUsersAndRolesExistAsync(userManager, roleManager);

                await EnsureCategoriesExistAsync(context);
                await EnsureBrandsExistAsync(context);
                await EnsureSizeExistAsync(context);
                await EnsureColorsExistAsync(context);

                await EnsureProductsExistAsync(context);
                await EnsureModelsExistAsync(context);
                await EnsureModelSizesExistAsync(context);
                await EnsurePhotosExistAsync(context); // Добавляем фото

                await EnsureAddressesExistAsync(context);
                await EnsureCommentsExistAsync(context);

                // Добавление заказов
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
    private static async Task EnsureSizeExistAsync(ShopApplicationContext context)
    {
        if (!context.Sizes.Any())
        {
            context.Sizes.AddRange(
                new Size { Name = "38" },
                new Size { Name = "39" },
                new Size { Name = "40" }
            );
            await context.SaveChangesAsync();
        }
    }
    private static async Task EnsureColorsExistAsync(ShopApplicationContext context)
    {
        if (!context.Colors.Any())
        {
            context.Colors.AddRange(
                new Color { Name = "Black" },
                new Color { Name = "White" },
                new Color { Name = "Blue" }
            );
            await context.SaveChangesAsync();
        }
    }
    private static async Task EnsureProductsExistAsync(ShopApplicationContext context)
    {
        if (!context.Products.Any())
        {
            var products = new List<Product>();
            
            // var sizes = new int[] { 38, 39, 40, 41, 42, 43 }; // Пример доступных размеров
            // var colors = new[] { "Red", "Blue", "Green", "Black", "White" }; // Пример цветов
            // var brands = new[] { "Nike", "Adidas", "Puma", "Reebok" }; // Пример брендов
            // var materials = new[] { "Leather", "Synthetic", "Mesh" }; // Пример материалов

            for (int i = 1; i <= 25; i++)
            {
                products.Add(new Product
                {
                    Id = i,
                    Name = $"Shoes {i}",
                    Description = $"Description for Product {i}",
                    CategoryId = 1, 
                    BrandId = 1,
                    Created = DateTime.UtcNow
                });
            }

            for (int i = 26; i <= 50; i++)
            {
                products.Add(new Product
                {
                    Id = i,
                    Name = $"Shoes {i}",
                    Description = $"Description for Product {i}",
                    CategoryId = 2,
                    BrandId = 2, 
                    Created = DateTime.UtcNow
                });
            }

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }

    private static async Task EnsureModelsExistAsync(ShopApplicationContext context)
    {
        if (!context.Models.Any())
        {
            context.Models.AddRange(
                new Model
                {
                    ColorId = 1,
                    ProductId = 1,
                    Price = 100,
                    Photos = new List<Photo> // Добавляем фото для модели
                    {
                        new Photo
                        {
                            FileName = "model1-photo1.jpg",
                            FilePath = "/images/model1-photo1.jpg",
                            Length = 2048
                        },
                        new Photo
                        {
                            FileName = "model1-photo2.jpg",
                            FilePath = "/images/model1-photo2.jpg",
                            Length = 2048
                        }
                    }
                },
                new Model
                {
                    ColorId = 2,
                    ProductId = 2,
                    Price = 200,
                    Photos = new List<Photo> // Добавляем фото для модели
                    {
                        new Photo
                        {
                            FileName = "model2-photo1.jpg",
                            FilePath = "/images/model2-photo1.jpg",
                            Length = 1024
                        }
                    }
                }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task EnsurePhotosExistAsync(ShopApplicationContext context)
    {
        if (!context.Photos.Any())
        {
            context.Photos.AddRange(
                new Photo
                {
                    FileName = "photo1.jpg",
                    FilePath = "/images/photo1.jpg",
                    Length = 2048,
                    ModelId = 1
                },
                new Photo
                {
                    FileName = "photo2.jpg",
                    FilePath = "/images/photo2.jpg",
                    Length = 1024,
                    ModelId = 2
                }
            );
            await context.SaveChangesAsync();
        }
    }

    
    private static async Task EnsureModelSizesExistAsync(ShopApplicationContext context)
    {
        if (!context.ModelSizes.Any())
        {
            context.ModelSizes.AddRange(
                new ModelSize
                {
                    ModelId = 1,
                    SizeId = 1
                },
                new ModelSize  
                {
                    ModelId = 2,
                    SizeId = 2
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
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com");

            context.Comments.AddRange(
                new Comment
                {
                    Id = 1,
                    Text = "Great product! Fast shipping!",
                    Created = DateTime.UtcNow,
                    ProductId = 1,
                    UserId = user.Id
                },
                new Comment
                {
                    Id = 2,
                    Text = "Good price for quality.",   
                    Created = DateTime.UtcNow.AddDays(-7),
                    ProductId = 2,
                    UserId = user.Id
                }
            );
            await context.SaveChangesAsync();
        }
    }

    private static async Task EnsureOrdersExistAsync(ShopApplicationContext context)
    {
        // if (!context.Orders.Any())
        // {
        //     var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com");
        //     var model1 = await context.Models.FirstOrDefaultAsync(m => m.ProductId == 1);
        //     var model2 = await context.Models.FirstOrDefaultAsync(m => m.ProductId == 2);
        //
        //     if (user != null && model1 != null && model2 != null)
        //     {
        //         var orders = new List<Order>
        //         {
        //             new Order
        //             {
        //                 UserId = user.Id,
        //                 Created = DateTime.UtcNow,
        //                 Status = OrderStatus.Processed,
        //                 TotalAmount = 300,  // пример суммы
        //                 OrderItems = new List<OrderItem>
        //                 {
        //                     new OrderItem
        //                     {
        //                         ModelId = model1.Id,
        //                         Quantity = 1,
        //                         Amount = model1.Price
        //                     },
        //                     new OrderItem
        //                     {
        //                         ModelId = model2.Id,
        //                         Quantity = 2,
        //                         Amount = model2.Price
        //                     }
        //                 }
        //             },
        //             new Order
        //             {
        //                 UserId = user.Id,
        //                 Created = DateTime.UtcNow.AddDays(-10),
        //                 Status = OrderStatus.Delivered,
        //                 TotalAmount = 400,  // пример суммы
        //                 OrderItems = new List<OrderItem>
        //                 {
        //                     new OrderItem
        //                     {
        //                         ModelId = model2.Id,
        //                         Quantity = 1,
        //                         Amount = model2.Price
        //                     }
        //                 }
        //             }
        //         };
        //
        //         context.Orders.AddRange(orders);
        //         await context.SaveChangesAsync();
        //     }
        // }

        if (!context.Orders.Any())
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com");
            var size1 = await context.Sizes.FirstOrDefaultAsync(s => s.Name == "38");
            var size2 = await context.Sizes.FirstOrDefaultAsync(s => s.Name == "39");
            var model1 = await context.Models.FirstOrDefaultAsync(m => m.ProductId == 1);
            var model2 = await context.Models.FirstOrDefaultAsync(m => m.ProductId == 2);

            if (size1 != null && size2 != null)
            {
                var orders = new List<Order>
                {
                    new Order
                    {
                        UserId = user.Id,
                        Created = DateTime.UtcNow,
                        Status = OrderStatus.Processed,
                        TotalAmount = 300,
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                ModelId = model1.Id,
                                Quantity = 1,
                                Amount = model1.Price,
                                SizeId = size1.Id // Указываем SizeId
                            },
                            new OrderItem
                            {
                                ModelId = model2.Id,
                                Quantity = 2,
                                Amount = model2.Price,
                                SizeId = size2.Id // Указываем SizeId
                            }
                        }
                    }
                };

                context.Orders.AddRange(orders);
                await context.SaveChangesAsync();
            }
        }

    }

}
