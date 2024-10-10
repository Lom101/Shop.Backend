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
                    Name = "Women's"
                },
                new Category
                {
                    Id = 2,
                    Name = "Men's"
                },
                new Category
                {
                    Id = 3,
                    Name = "Children's"
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
                new Brand { Name = "Puma" },
                new Brand { Name = "Reebok" }
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
                new Size { Name = "40" },
                new Size { Name = "41" },
                new Size { Name = "42" },
                new Size { Name = "43" }
            );
            await context.SaveChangesAsync();
        }
    }
    private static async Task EnsureColorsExistAsync(ShopApplicationContext context)
    {
        if (!context.Colors.Any())
        {
            context.Colors.AddRange(
                new Color { Name = "Red" },
                new Color { Name = "Blue" },
                new Color { Name = "Green" },
                new Color { Name = "Black" },
                new Color { Name = "White" }
            );
            await context.SaveChangesAsync();
        }
    }
    
    
    
    private static async Task EnsureProductsExistAsync(ShopApplicationContext context)
{
    if (!context.Products.Any())
    {
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Кроссовки Air Max 1",
                Description = "Классические кроссовки Nike Air Max 1 для активного отдыха.",
                CategoryId = 1, // ID категории "Кроссовки"
                BrandId = 1, // ID бренда "Nike"
                Created = DateTime.UtcNow
            },
            new Product
            {
                Id = 2,
                Name = "Кроссовки Ultraboost",
                Description = "Спортивные кроссовки Adidas Ultraboost для бега.",
                CategoryId = 1,
                BrandId = 2,
                Created = DateTime.UtcNow
            },
            new Product
            {
                Id = 3,
                Name = "Кроссовки Puma RS-X",
                Description = "Современные кроссовки Puma RS-X с ярким дизайном.",
                CategoryId = 1,
                BrandId = 3,
                Created = DateTime.UtcNow
            },
            new Product
            {
                Id = 4,
                Name = "Кроссовки Reebok Classic",
                Description = "Классические кроссовки Reebok для повседневной носки.",
                CategoryId = 2, // ID категории "Спортивная обувь"
                BrandId = 4,
                Created = DateTime.UtcNow
            },
            new Product
            {
                Id = 5,
                Name = "Кроссовки New Balance 574",
                Description = "Элегантные кроссовки New Balance 574 для города.",
                CategoryId = 2,
                BrandId = 1,
                Created = DateTime.UtcNow
            },
            new Product
            {
                Id = 6,
                Name = "Кроссовки Nike Air Force 1",
                Description = "Классические кроссовки Nike Air Force 1, подходящие для любого образа.",
                CategoryId = 3, // ID категории "Повседневная обувь"
                BrandId = 1,
                Created = DateTime.UtcNow
            },
            new Product
            {
                Id = 7,
                Name = "Кроссовки Adidas NMD",
                Description = "Модные кроссовки Adidas NMD для стильного повседневного образа.",
                CategoryId = 3,
                BrandId = 2,
                Created = DateTime.UtcNow
            },
            new Product
            {
                Id = 8,
                Name = "Кроссовки Puma Suede",
                Description = "Классические кроссовки Puma Suede для уличного стиля.",
                CategoryId = 3,
                BrandId = 3,
                Created = DateTime.UtcNow
            }
        };

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
                ColorId = 1, // Например, Black
                ProductId = 1, // Кроссовки Air Max 1
                Price = 100,
                Photos = new List<Photo> // Добавляем фото для модели
                {
                    new Photo
                    {
                        FileName = "air_max_1_black.jpg",
                        FilePath = "/images/air_max_1_black.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 2, // Например, White
                ProductId = 1, // Кроссовки Air Max 1
                Price = 110,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "air_max_1_white.jpg",
                        FilePath = "/images/air_max_1_white.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 3, // Например, Blue
                ProductId = 2, // Кроссовки Ultraboost
                Price = 200,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "ultraboost_blue.jpg",
                        FilePath = "/images/ultraboost_blue.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 4, // Например, Red
                ProductId = 2, // Кроссовки Ultraboost
                Price = 210,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "ultraboost_red.jpg",
                        FilePath = "/images/ultraboost_red.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 1, // Например, Black
                ProductId = 3, // Кроссовки Puma RS-X
                Price = 120,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "puma_rsx_black.jpg",
                        FilePath = "/images/puma_rsx_black.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 5, // Например, White
                ProductId = 3, // Кроссовки Puma RS-X
                Price = 130,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "puma_rsx_white.jpg",
                        FilePath = "/images/puma_rsx_white.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 3, // Например, Blue
                ProductId = 4, // Кроссовки Reebok Classic
                Price = 90,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "reebok_classic_blue.jpg",
                        FilePath = "/images/reebok_classic_blue.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 4, // Например, Red
                ProductId = 4, // Кроссовки Reebok Classic
                Price = 95,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "reebok_classic_red.jpg",
                        FilePath = "/images/reebok_classic_red.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 2, // Например, White
                ProductId = 5, // Кроссовки New Balance 574
                Price = 110,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "new_balance_574_white.jpg",
                        FilePath = "/images/new_balance_574_white.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 1, // Например, Black
                ProductId = 6, // Кроссовки Nike Air Force 1
                Price = 120,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "air_force_1_black.jpg",
                        FilePath = "/images/air_force_1_black.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 5, // Например, White
                ProductId = 6, // Кроссовки Nike Air Force 1
                Price = 125,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "air_force_1_white.jpg",
                        FilePath = "/images/air_force_1_white.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 3, // Например, Blue
                ProductId = 7, // Кроссовки Adidas NMD
                Price = 140,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "nmd_blue.jpg",
                        FilePath = "/images/nmd_blue.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 4, // Например, Red
                ProductId = 7, // Кроссовки Adidas NMD
                Price = 145,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "nmd_red.jpg",
                        FilePath = "/images/nmd_red.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 1, // Например, Black
                ProductId = 8, // Кроссовки Puma Suede
                Price = 110,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "puma_suede_black.jpg",
                        FilePath = "/images/puma_suede_black.jpg",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 5, // Например, White
                ProductId = 8, // Кроссовки Puma Suede
                Price = 115,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "puma_suede_white.jpg",
                        FilePath = "/images/puma_suede_white.jpg",
                        Length = 2048
                    }
                }
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
                // Air Max 1 доступные размеры 38, 39, 40
                new ModelSize { ModelId = 1, SizeId = 1, StockQuantity = 10 }, // Size 38
                new ModelSize { ModelId = 1, SizeId = 2, StockQuantity = 5 },  // Size 39
                new ModelSize { ModelId = 1, SizeId = 3, StockQuantity = 0 },  // Size 40 (недоступен)

                // Ultraboost доступные размеры 39, 40, 41
                new ModelSize { ModelId = 2, SizeId = 2, StockQuantity = 12 }, // Size 39
                new ModelSize { ModelId = 2, SizeId = 3, StockQuantity = 8 },  // Size 40
                new ModelSize { ModelId = 2, SizeId = 4, StockQuantity = 6 },  // Size 41

                // Puma RS-X доступные размеры 38, 40, 41
                new ModelSize { ModelId = 3, SizeId = 1, StockQuantity = 5 },  // Size 38
                new ModelSize { ModelId = 3, SizeId = 3, StockQuantity = 3 },  // Size 40
                new ModelSize { ModelId = 3, SizeId = 4, StockQuantity = 0 },  // Size 41 (недоступен)

                // Reebok Classic доступные размеры 39, 40, 41, 42
                new ModelSize { ModelId = 4, SizeId = 2, StockQuantity = 10 }, // Size 39
                new ModelSize { ModelId = 4, SizeId = 3, StockQuantity = 5 },  // Size 40
                new ModelSize { ModelId = 4, SizeId = 4, StockQuantity = 0 },  // Size 41 (недоступен)
                new ModelSize { ModelId = 4, SizeId = 5, StockQuantity = 7 },  // Size 42

                // New Balance 574 доступные размеры 38, 40, 42
                new ModelSize { ModelId = 5, SizeId = 1, StockQuantity = 8 },  // Size 38
                new ModelSize { ModelId = 5, SizeId = 3, StockQuantity = 0 },  // Size 40 (недоступен)
                new ModelSize { ModelId = 5, SizeId = 5, StockQuantity = 6 },  // Size 42

                // Nike Air Force 1 доступные размеры 39, 41, 43
                new ModelSize { ModelId = 6, SizeId = 2, StockQuantity = 12 }, // Size 39
                new ModelSize { ModelId = 6, SizeId = 4, StockQuantity = 0 },  // Size 41 (недоступен)
                new ModelSize { ModelId = 6, SizeId = 6, StockQuantity = 5 },  // Size 43

                // Adidas NMD доступные размеры 38, 39, 41
                new ModelSize { ModelId = 7, SizeId = 1, StockQuantity = 4 },  // Size 38
                new ModelSize { ModelId = 7, SizeId = 2, StockQuantity = 10 }, // Size 39
                new ModelSize { ModelId = 7, SizeId = 4, StockQuantity = 0 },  // Size 41 (недоступен)

                // Puma Suede доступные размеры 40, 41, 42, 43
                new ModelSize { ModelId = 8, SizeId = 3, StockQuantity = 2 },  // Size 40
                new ModelSize { ModelId = 8, SizeId = 4, StockQuantity = 0 },  // Size 41 (недоступен)
                new ModelSize { ModelId = 8, SizeId = 5, StockQuantity = 3 },  // Size 42
                new ModelSize { ModelId = 8, SizeId = 6, StockQuantity = 1 }   // Size 43
            );

            await context.SaveChangesAsync();
        }
    }   

    
    
    private static async Task EnsurePhotosExistAsync(ShopApplicationContext context)
    {
        // if (!context.Photos.Any())
        // {
        //     context.Photos.AddRange(
        //         new Photo
        //         {
        //             FileName = "photo1.jpg",
        //             FilePath = "/images/photo1.jpg",
        //             Length = 2048,
        //             ModelId = 1
        //         },
        //         new Photo
        //         {
        //             FileName = "photo2.jpg",
        //             FilePath = "/images/photo2.jpg",
        //             Length = 1024,
        //             ModelId = 2
        //         }
        //     );
        //     await context.SaveChangesAsync();
        // }
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
                    AddressName = "123 Main St",
                    UserId = user.Id
                },
                new Address
                {
                    Id = 2,
                    AddressName = "456 Elm Ave",
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
                    Rating = 5,
                    Created = DateTime.UtcNow,
                    ProductId = 1,
                    UserId = user.Id
                },
                new Comment
                {
                    Id = 2,
                    Text = "Good price for quality.",   
                    Rating = 4,
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
    if (!context.Orders.Any())
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com");

        // Получаем адрес пользователя для использования в заказах
        var address = await context.Addresses.FirstOrDefaultAsync(a => a.UserId == user.Id);

        // Извлекаем модели вместе с размерами через ModelSize
        var model1 = await context.Models
            .Include(m => m.ModelSizes)
                .ThenInclude(ms => ms.Size)
            .FirstOrDefaultAsync(m => m.ProductId == 1);

        var model2 = await context.Models
            .Include(m => m.ModelSizes)
                .ThenInclude(ms => ms.Size)
            .FirstOrDefaultAsync(m => m.ProductId == 2);

        if (model1 != null && model2 != null && model1.ModelSizes.Any() && model2.ModelSizes.Any())
        {
            var size1 = model1.ModelSizes.FirstOrDefault()?.Size;
            var size2 = model2.ModelSizes.FirstOrDefault()?.Size;

            if (size1 != null && size2 != null && address != null)
            {
                var orders = new List<Order>
                {
                    new Order
                    {
                        UserId = user.Id,
                        Created = DateTime.UtcNow,
                        Status = OrderStatus.Processed,
                        PaymentIntentId = "pi_1GqjYf2eZvKYlo2C8p1JQY1M", // Пример ID платежного намерения
                        AddressId = address.Id, // Связываем с адресом
                        ContactPhone = "123-456-7890", // Пример контактного телефона
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                ModelId = model1.Id,
                                Quantity = 1,
                                Amount = model1.Price,
                                SizeId = size1.Id
                            },
                            new OrderItem
                            {
                                ModelId = model2.Id,
                                Quantity = 2,
                                Amount = model2.Price,
                                SizeId = size2.Id
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

}
