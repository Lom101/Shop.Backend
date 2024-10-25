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
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
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
                //await EnsurePhotosExistAsync(context); // Добавляем фото
                
                // await EnsureAddressesExistAsync(context);
                await EnsureCommentsExistAsync(context);
                //
                // // Добавление заказов
                // await EnsureOrdersExistAsync(context); 

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
    private static async Task EnsureUsersAndRolesExistAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var adminUser = await userManager.FindByEmailAsync("admin@example.com");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser { UserName = "admin", Email = "admin@example.com" };
            await userManager.CreateAsync(adminUser, "Admin@123");
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        var normalUser = await userManager.FindByEmailAsync("user@example.com");
        if (normalUser == null)
        {
            normalUser = new ApplicationUser { UserName = "user", Email = "user@example.com" };
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
                    Name = "Women's"
                },
                new Category
                {
                    Name = "Men's"
                },
                new Category
                {
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
                new Brand { Name = "Reebok" },
                new Brand { Name = "New Balance" },
                new Brand { Name = "Under Armour" },
                new Brand { Name = "Asics" },
                new Brand { Name = "Saucony" }
            );
            await context.SaveChangesAsync();
        }
    }
    private static async Task EnsureSizeExistAsync(ShopApplicationContext context)
    {
        if (!context.Sizes.Any())
        {
            context.Sizes.AddRange(
                new Size { Name = "36" },
                new Size { Name = "37" },
                new Size { Name = "38" },
                new Size { Name = "39" },
                new Size { Name = "40" },
                new Size { Name = "41" },
                new Size { Name = "42" },
                new Size { Name = "43" },
                new Size { Name = "44" }, 
                new Size { Name = "45" },
                new Size { Name = "46" }
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
                new Color { Name = "White" },
                new Color { Name = "Yellow" },
                new Color { Name = "Orange" },
                new Color { Name = "Purple" },
                new Color { Name = "Pink" },
                new Color { Name = "Gray" }
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
                Name = "Nike Air Max 1",
                Description = "Кроссовки Nike Air Max 1 представляют собой идеальное сочетание стиля и комфорта. Оснащенные легендарной амортизацией Air, они отлично подходят как для активного отдыха, так и для повседневной носки. Эти кроссовки имеют современный и лаконичный дизайн, который станет отличным дополнением к любому наряду. Идеальны для женщин, которые ценят как моду, так и комфорт.",
                CategoryId = 1, // ID категории "Women's"
                BrandId = 1, // ID бренда "Nike"
                Created = DateTime.UtcNow
            },
            new Product
            {
                Name = "Asics Gel Nyc",
                Description = "Asics Gel-NYC — это кроссовки, сочетающие в себе классический дизайн и современные технологии, обеспечивая комфорт и поддержку на каждом шагу. Их стиль вдохновлен архивными моделями Asics, такими как GEL-Nimbus 3 и GEL-MC Plus, с динамичными линиями и утонченными деталями.\n\n",
                CategoryId = 2, // ID категории "Men's"
                BrandId = 7, // ID бренда "Asics"
                Created = DateTime.UtcNow
            },
            new Product
            {
                Name = "Puma Caven 2.0",
                Description = "Puma Caven 2.0 — это стильные кроссовки, сочетающие в себе классический баскетбольный стиль и современные элементы, что делает их идеальными для повседневного ношения. Силуэт напоминает классическую спортивную обувь Puma, с элегантными линиями и минималистичным дизайном.",
                CategoryId = 1, // ID категории "Men's"
                BrandId = 3,
                Created = DateTime.UtcNow
            },
            new Product
            {
                Name = "Reebok Classic Leather",
                Description = "Классические кроссовки Reebok Classic Leather являются символом стиля и комфорта. Изготовленные из натуральной кожи, они идеально подходят для мужчин, которые ищут надежную и стильную обувь для повседневной носки. Эти кроссовки легко сочетаются с различными нарядами и подчеркивают индивидуальность владельца.",
                CategoryId = 2, // ID категории "Men's"
                BrandId = 4,
                Created = DateTime.UtcNow
            },
            new Product
            {
                Name = "New Balance 574",
                Description = "Элегантные кроссовки New Balance 574 идеально подходят для женщин, которые ценят стиль и комфорт. Их классический дизайн и высококачественные материалы обеспечивают долговечность и удобство, что делает их отличным выбором для прогулок по городу или активного отдыха.",
                CategoryId = 1, // ID категории "Women's"
                BrandId = 5,
                Created = DateTime.UtcNow
            },
            new Product
            {
                Name = "Nike Air Force 1 '07",
                Description = "Кроссовки Nike Air Force 1 '07 — это стильный и универсальный выбор для мужчин. С их классическим дизайном и надежной амортизацией они подходят как для тренировок, так и для повседневной носки. Эти кроссовки подчеркнут ваш индивидуальный стиль и обеспечат комфорт в течение всего дня.",
                CategoryId = 2, // ID категории "Men's"
                BrandId = 1,
                Created = DateTime.UtcNow
            },
            new Product
            {
                Name = "Adidas Samba OG",
                Description = "Adidas Samba OG — это культовые кроссовки, ставшие классикой, благодаря своему ретро-дизайну и неизменному стилю. Изначально разработанные как футбольная обувь, они получили широкую популярность за пределами поля благодаря своему универсальному дизайну и комфорту.",
                CategoryId = 2, // ID категории "Men's"
                BrandId = 2,
                Created = DateTime.UtcNow
            },  
            new Product
            {
                Name = "Puma Suede XL",
                Description = "Классические кроссовки Puma Suede XL, выполненные из мягкой замши, предлагают уникальный стиль и комфорт для женщин. Эти кроссовки отлично смотрятся с любым нарядом и идеально подходят для повседневной носки. Их стильный дизайн делает их идеальным выбором для любых случаев.",
                CategoryId = 1, // ID категории "Women's"
                BrandId = 3,
                Created = DateTime.UtcNow
            },
            // new Product
            // {
            //     Id = 9,
            //     Name = "ASICS Gel-Kayano 28",
            //     Description = "Кроссовки для бега ASICS Gel-Kayano 28 предлагают отличную поддержку и амортизацию. Они идеально подходят для мужчин, которые занимаются бегом на длинные дистанции. Эти кроссовки обеспечивают комфорт и защиту от ударов, что делает их незаменимыми на тренировках.",
            //     CategoryId = 2, // ID категории "Men's"
            //     BrandId = 5, // ID бренда "ASICS"
            //     Created = DateTime.UtcNow
            // },
            // new Product
            // {
            //     Id = 10,
            //     Name = "Saucony Kinvara 13",
            //     Description = "Легкие кроссовки для бега Saucony Kinvara 13 созданы для активных детей и подростков. Они предлагают отличную гибкость и амортизацию, что делает их идеальными для тренировок и игр на свежем воздухе. Эти кроссовки помогут юным спортсменам достичь новых высот.",
            //     CategoryId = 3, // ID категории "Children's"
            //     BrandId = 6, // ID бренда "Saucony"
            //     Created = DateTime.UtcNow
            // },
            // new Product
            // {
            //     Id = 11,
            //     Name = "Under Armour HOVR Phantom 2",
            //     Description = "Кроссовки Under Armour HOVR Phantom 2 обеспечивают высокий уровень комфорта и поддержки благодаря технологии HOVR. Идеальны для мужчин, которые активно занимаются спортом, они предлагают отличную амортизацию и стильный внешний вид.",
            //     CategoryId = 2, // ID категории "Men's"
            //     BrandId = 7, // ID бренда "Under Armour"
            //     Created = DateTime.UtcNow
            // },
            // new Product
            // {
            //     Id = 12,
            //     Name = "Nike React Infinity Run",
            //     Description = "Кроссовки Nike React Infinity Run обеспечивают идеальный баланс между амортизацией и откликом. Они предназначены для бегунов всех уровней и предлагают отличную поддержку на длинных дистанциях. Эти кроссовки подходят как для мужчин, так и для женщин, которые ценят комфорт и производительность.",
            //     CategoryId = 1, // ID категории "Women's"
            //     BrandId = 1,
            //     Created = DateTime.UtcNow
            // },
            // new Product
            // {
            //     Id = 13,
            //     Name = "Adidas Gazelle",
            //     Description = "Кроссовки Adidas Gazelle представляют собой классический стиль, который подходит для всех возрастов. Эти кроссовки легко комбинируются с различной одеждой, обеспечивая непринужденный и стильный вид как для мужчин, так и для женщин. Идеальны для повседневной носки и отдыха.",
            //     CategoryId = 1, // ID категории "Women's"
            //     BrandId = 2,
            //     Created = DateTime.UtcNow
            // },
            // new Product
            // {
            //     Id = 14,
            //     Name = "New Balance Fresh Foam 1080v11",
            //     Description = "Кроссовки New Balance Fresh Foam 1080v11 предлагают максимальный комфорт и поддержку для мужчин, занимающихся бегом. Их уникальная конструкция и отличная амортизация позволяют ощущать себя комфортно на протяжении всего дня, делая их идеальным выбором для активных людей.",
            //     CategoryId = 2, // ID категории "Men's"
            //     BrandId = 1,
            //     Created = DateTime.UtcNow
            // },
            // new Product
            // {
            //     Id = 15,
            //     Name = "Hoka One One Bondi 7",
            //     Description = "Кроссовки Hoka One One Bondi 7 с максимальной амортизацией предназначены для всех, кто ищет комфорт в каждом шаге. Эти кроссовки идеально подходят для длительных прогулок и восстановления после тренировок. Они подходят как для мужчин, так и для женщин, обеспечивая надежную поддержку.",
            //     CategoryId = 1, // ID категории "Women's"
            //     BrandId = 8, // ID бренда "Hoka One One"
            //     Created = DateTime.UtcNow
            // }
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
            // Модели для кроссовок Air Max 1 
            new Model
            {
                ColorId = 4, // Black
                ProductId = 1, // Air Max 1
                Price = 5000,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "air_max_1_black.webp",
                        FilePath = "/images/air_max_1_black.webp",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 5, // White
                ProductId = 1, // Air Max 1
                Price = 5000,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "air_max_1_white.webp",
                        FilePath = "/images/air_max_1_white.webp",
                        Length = 2048
                    }
                }
            },

            // Модели для Asics Gel Nyc
            new Model
            {
                ColorId = 10, // Grey
                ProductId = 2, // Asics Gel Nyc
                Price = 7000,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "gel_nyc_grey.webp",
                        FilePath = "/images/gel_nyc_grey.webp",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 4, // Black
                ProductId = 2, // Asics Gel Nyc
                Price = 7500,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "gel_nyc_black.webp",
                        FilePath = "/images/gel_nyc_black.webp",
                        Length = 2048
                    }
                }
            },

            // Модели для Puma Caven 2.0
            new Model
            {
                ColorId = 4, // Black
                ProductId = 3, // Puma Caven 2.0
                Price = 4300,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "puma_caven_2_0_black.webp",
                        FilePath = "/images/puma_caven_2_0_black.webp",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 5, // White
                ProductId = 3, // Puma Caven 2.0
                Price = 4400,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "puma_caven_2_0_white.webp",
                        FilePath = "/images/puma_caven_2_0_white.webp",
                        Length = 2048
                    }
                }
            },

            // Модели для Reebok Classic
            new Model
            {
                ColorId = 4, // Black
                ProductId = 4, // Reebok Classic
                Price = 4600,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "reebok_classic_black.webp",
                        FilePath = "/images/reebok_classic_black.webp",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 5, // White
                ProductId = 4, // Reebok Classic
                Price = 4700,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "reebok_classic_white.webp",
                        FilePath = "/images/reebok_classic_white.webp",
                        Length = 2048
                    }
                }
            },

            // Модели для New Balance 574
            new Model
            {
                ColorId = 5, // White
                ProductId = 5, // New Balance 574
                Price = 5300,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "new_balance_574_white.webp",
                        FilePath = "/images/new_balance_574_white.webp",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 4, // Black
                ProductId = 5, // New Balance 574
                Price = 5300,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "new_balance_574_black.webp",
                        FilePath = "/images/new_balance_574_black.webp",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 10, // Grey
                ProductId = 5, // New Balance 574
                Price = 5300,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "new_balance_574_grey.webp",
                        FilePath = "/images/new_balance_574_grey.webp",
                        Length = 2048
                    }
                }
            },
            
            // Модели для Nike Air Force 1 '07
            new Model
            {
                ColorId = 4, // Black
                ProductId = 6, // Nike Air Force 1 '07
                Price = 5500,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "air_force_1_07_black.webp",
                        FilePath = "/images/air_force_1_07_black.webp",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 3, // Green
                ProductId = 6, // Nike Air Force 1 '07
                Price = 5800,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "air_force_1_07_green.webp",
                        FilePath = "/images/air_force_1_07_green.webp",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 5, // White
                ProductId = 6, // Nike Air Force 1 '07
                Price = 5400,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "air_force_1_07_white.webp",
                        FilePath = "/images/air_force_1_07_white.webp",
                        Length = 2048
                    }
                }
            },

            // Модели для Adidas Samba OG
            new Model
            {
                ColorId = 2, // Blue
                ProductId = 7, // Adidas Samba OG
                Price = 3500,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "adidas_samba_og_blue.webp",
                        FilePath = "/images/adidas_samba_og_blue.webp",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 1, // Red
                ProductId = 7, // Adidas Samba OG
                Price = 3600,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "adidas_samba_og_green.webp",
                        FilePath = "/images/adidas_samba_og_green.webp",
                        Length = 2048
                    }
                }
            },

            // Модели для Puma Suede XL
            new Model
            {
                ColorId = 3, // Green
                ProductId = 8, // Puma Suede XL
                Price = 4000,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "puma_suede_xl_green.webp",
                        FilePath = "/images/puma_suede_xl_green.webp",
                        Length = 2048
                    }
                }
            },
            new Model
            {
                ColorId = 5, // White
                ProductId = 8, // Puma Suede XL
                Price = 4600,
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        FileName = "puma_suede_xl_white.webp",
                        FilePath = "/images/puma_suede_xl_white.webp",
                        Length = 2048
                    }
                }
            }

            // // Новые модели кроссовок
            // new Model
            // {
            //     ColorId = 2, // White
            //     ProductId = 9, // Hoka One One Bondi 7
            //     Price = 160,
            //     Photos = new List<Photo>
            //     {
            //         new Photo
            //         {
            //             FileName = "hoka_bondi_7_white.webp",
            //             FilePath = "/images/hoka_bondi_7_white.webp",
            //             Length = 2048
            //         }
            //     }
            // },
            // new Model
            // {
            //     ColorId = 1, // Black
            //     ProductId = 9, // Hoka One One Bondi 7
            //     Price = 165,
            //     Photos = new List<Photo>
            //     {
            //         new Photo
            //         {
            //             FileName = "hoka_bondi_7_black.webp",
            //             FilePath = "/images/hoka_bondi_7_black.webp",
            //             Length = 2048
            //         }
            //     }
            // },
            // new Model
            // {
            //     ColorId = 3, // Blue
            //     ProductId = 10, // Asics Gel-Kayano
            //     Price = 180,
            //     Photos = new List<Photo>
            //     {
            //         new Photo
            //         {
            //             FileName = "asics_gel_kayano_blue.webp",
            //             FilePath = "/images/asics_gel_kayano_blue.webp",
            //             Length = 2048
            //         }
            //     }
            // },
            // new Model
            // {
            //     ColorId = 4, // Red
            //     ProductId = 10, // Asics Gel-Kayano
            //     Price = 185,
            //     Photos = new List<Photo>
            //     {
            //         new Photo
            //         {
            //             FileName = "asics_gel_kayano_red.webp",
            //             FilePath = "/images/asics_gel_kayano_red.webp",
            //             Length = 2048
            //         }
            //     }
            // },
            // new Model
            // {
            //     ColorId = 2, // White
            //     ProductId = 11, // Mizuno Wave Rider
            //     Price = 170,
            //     Photos = new List<Photo>
            //     {
            //         new Photo
            //         {
            //             FileName = "mizuno_wave_rider_white.webp",
            //             FilePath = "/images/mizuno_wave_rider_white.webp",
            //             Length = 2048
            //         }
            //     }
            // },
            // new Model
            // {
            //     ColorId = 1, // Black
            //     ProductId = 12, // Saucony Triumph
            //     Price = 175,
            //     Photos = new List<Photo>
            //     {
            //         new Photo
            //         {
            //             FileName = "saucony_triumph_black.webp",
            //             FilePath = "/images/saucony_triumph_black.webp",
            //             Length = 2048
            //         }
            //     }
            // },
            // new Model
            // {
            //     ColorId = 5, // White
            //     ProductId = 12, // Saucony Triumph
            //     Price = 180,
            //     Photos = new List<Photo>
            //     {
            //         new Photo
            //         {
            //             FileName = "saucony_triumph_white.webp",
            //             FilePath = "/images/saucony_triumph_white.webp",
            //             Length = 2048
            //         }
            //     }
            // }
        );

        await context.SaveChangesAsync();
    }
}
    private static async Task EnsureModelSizesExistAsync(ShopApplicationContext context)
    {
        if (!context.ModelSizes.Any())
        {
            context.ModelSizes.AddRange(
            // Air Max 1 доступные размеры 38, 39, 40, 41, 42
            new ModelSize { ModelId = 1, SizeId = 1, StockQuantity = 10 }, // Size 38
            new ModelSize { ModelId = 1, SizeId = 2, StockQuantity = 5 },  // Size 39
            new ModelSize { ModelId = 1, SizeId = 3, StockQuantity = 0 },  // Size 40 (недоступен)
            new ModelSize { ModelId = 1, SizeId = 4, StockQuantity = 8 },  // Size 41
            new ModelSize { ModelId = 1, SizeId = 5, StockQuantity = 12 }, // Size 42

            // Ultraboost доступные размеры 39, 40, 41, 42, 43
            new ModelSize { ModelId = 2, SizeId = 2, StockQuantity = 12 }, // Size 39
            new ModelSize { ModelId = 2, SizeId = 3, StockQuantity = 8 },  // Size 40
            new ModelSize { ModelId = 2, SizeId = 4, StockQuantity = 6 },  // Size 41
            new ModelSize { ModelId = 2, SizeId = 5, StockQuantity = 10 }, // Size 42
            new ModelSize { ModelId = 2, SizeId = 6, StockQuantity = 4 },  // Size 43

            // Puma RS-X доступные размеры 38, 39, 40, 41, 42
            new ModelSize { ModelId = 3, SizeId = 1, StockQuantity = 5 },  // Size 38
            new ModelSize { ModelId = 3, SizeId = 2, StockQuantity = 4 },  // Size 39
            new ModelSize { ModelId = 3, SizeId = 3, StockQuantity = 3 },  // Size 40
            new ModelSize { ModelId = 3, SizeId = 4, StockQuantity = 0 },  // Size 41 (недоступен)
            new ModelSize { ModelId = 3, SizeId = 5, StockQuantity = 2 },  // Size 42

            // Reebok Classic доступные размеры 39, 40, 41, 42, 43
            new ModelSize { ModelId = 4, SizeId = 2, StockQuantity = 10 }, // Size 39
            new ModelSize { ModelId = 4, SizeId = 3, StockQuantity = 5 },  // Size 40
            new ModelSize { ModelId = 4, SizeId = 4, StockQuantity = 0 },  // Size 41 (недоступен)
            new ModelSize { ModelId = 4, SizeId = 5, StockQuantity = 7 },  // Size 42
            new ModelSize { ModelId = 4, SizeId = 6, StockQuantity = 3 },  // Size 43

            // New Balance 574 доступные размеры 38, 39, 40, 41, 42
            new ModelSize { ModelId = 5, SizeId = 1, StockQuantity = 8 },  // Size 38
            new ModelSize { ModelId = 5, SizeId = 2, StockQuantity = 4 },  // Size 39
            new ModelSize { ModelId = 5, SizeId = 3, StockQuantity = 0 },  // Size 40 (недоступен)
            new ModelSize { ModelId = 5, SizeId = 5, StockQuantity = 6 },  // Size 42
            new ModelSize { ModelId = 5, SizeId = 6, StockQuantity = 5 },  // Size 43

            // Nike Air Force 1 доступные размеры 38, 39, 40, 41, 42, 43
            new ModelSize { ModelId = 6, SizeId = 1, StockQuantity = 10 }, // Size 38
            new ModelSize { ModelId = 6, SizeId = 2, StockQuantity = 12 }, // Size 39
            new ModelSize { ModelId = 6, SizeId = 3, StockQuantity = 6 },  // Size 40
            new ModelSize { ModelId = 6, SizeId = 4, StockQuantity = 0 },  // Size 41 (недоступен)
            new ModelSize { ModelId = 6, SizeId = 5, StockQuantity = 5 },  // Size 42
            new ModelSize { ModelId = 6, SizeId = 6, StockQuantity = 5 },  // Size 43

            // Adidas NMD доступные размеры 38, 39, 40, 41, 42
            new ModelSize { ModelId = 7, SizeId = 1, StockQuantity = 4 },  // Size 38
            new ModelSize { ModelId = 7, SizeId = 2, StockQuantity = 10 }, // Size 39
            new ModelSize { ModelId = 7, SizeId = 3, StockQuantity = 5 },  // Size 40
            new ModelSize { ModelId = 7, SizeId = 4, StockQuantity = 2 },  // Size 41
            new ModelSize { ModelId = 7, SizeId = 5, StockQuantity = 0 },  // Size 42 (недоступен)

            // Puma Suede доступные размеры 39, 40, 41, 42, 43
            new ModelSize { ModelId = 8, SizeId = 2, StockQuantity = 2 },  // Size 39
            new ModelSize { ModelId = 8, SizeId = 3, StockQuantity = 0 },  // Size 40 (недоступен)
            new ModelSize { ModelId = 8, SizeId = 4, StockQuantity = 3 },  // Size 41
            new ModelSize { ModelId = 8, SizeId = 5, StockQuantity = 1 },  // Size 42
            new ModelSize { ModelId = 8, SizeId = 6, StockQuantity = 1 },  // Size 43

            // Nike React доступные размеры 38, 39, 40, 41, 42, 43
            new ModelSize { ModelId = 9, SizeId = 1, StockQuantity = 8 },  // Size 38
            new ModelSize { ModelId = 9, SizeId = 2, StockQuantity = 7 },  // Size 39
            new ModelSize { ModelId = 9, SizeId = 3, StockQuantity = 5 },  // Size 40
            new ModelSize { ModelId = 9, SizeId = 4, StockQuantity = 2 },  // Size 41
            new ModelSize { ModelId = 9, SizeId = 5, StockQuantity = 1 },  // Size 42
            new ModelSize { ModelId = 9, SizeId = 6, StockQuantity = 0 },  // Size 43 (недоступен)

            // New Balance 990 доступные размеры 39, 40, 41, 42, 43
            new ModelSize { ModelId = 10, SizeId = 2, StockQuantity = 10 }, // Size 39
            new ModelSize { ModelId = 10, SizeId = 3, StockQuantity = 8 },  // Size 40
            new ModelSize { ModelId = 10, SizeId = 4, StockQuantity = 0 },  // Size 41 (недоступен)
            new ModelSize { ModelId = 10, SizeId = 5, StockQuantity = 5 },  // Size 42
            new ModelSize { ModelId = 10, SizeId = 6, StockQuantity = 4 },  // Size 43

            // Adidas Superstar доступные размеры 38, 39, 40, 41, 42
            new ModelSize { ModelId = 11, SizeId = 1, StockQuantity = 6 },  // Size 38
            new ModelSize { ModelId = 11, SizeId = 2, StockQuantity = 12 }, // Size 39
            new ModelSize { ModelId = 11, SizeId = 3, StockQuantity = 9 },  // Size 40
            new ModelSize { ModelId = 11, SizeId = 4, StockQuantity = 3 },  // Size 41
            new ModelSize { ModelId = 11, SizeId = 5, StockQuantity = 2 },  // Size 42

            // Converse Chuck Taylor доступные размеры 39, 40, 41, 42, 43
            new ModelSize { ModelId = 12, SizeId = 2, StockQuantity = 10 }, // Size 39
            new ModelSize { ModelId = 12, SizeId = 3, StockQuantity = 5 },  // Size 40
            new ModelSize { ModelId = 12, SizeId = 4, StockQuantity = 4 },  // Size 41
            new ModelSize { ModelId = 12, SizeId = 5, StockQuantity = 6 },  // Size 42
            new ModelSize { ModelId = 12, SizeId = 6, StockQuantity = 0 },  // Size 43 (недоступен)

            // Asics Gel-Lyte доступные размеры 38, 39, 40, 41, 42
            new ModelSize { ModelId = 13, SizeId = 1, StockQuantity = 2 },  // Size 38
            new ModelSize { ModelId = 13, SizeId = 2, StockQuantity = 3 },  // Size 39
            new ModelSize { ModelId = 13, SizeId = 3, StockQuantity = 0 },  // Size 40 (недоступен)
            new ModelSize { ModelId = 13, SizeId = 4, StockQuantity = 4 },  // Size 41
            new ModelSize { ModelId = 13, SizeId = 5, StockQuantity = 6 },  // Size 42

            // Saucony Shadow доступные размеры 39, 40, 41, 42
            new ModelSize { ModelId = 14, SizeId = 2, StockQuantity = 4 },  // Size 39
            new ModelSize { ModelId = 14, SizeId = 3, StockQuantity = 2 },  // Size 40
            new ModelSize { ModelId = 14, SizeId = 4, StockQuantity = 6 },  // Size 41
            new ModelSize { ModelId = 14, SizeId = 5, StockQuantity = 0 },  // Size 42 (недоступен)

            // Nike Air Max 90 доступные размеры 38, 39, 40, 41, 42
            new ModelSize { ModelId = 15, SizeId = 1, StockQuantity = 5 },  // Size 38
            new ModelSize { ModelId = 15, SizeId = 2, StockQuantity = 8 },  // Size 39
            new ModelSize { ModelId = 15, SizeId = 3, StockQuantity = 3 },  // Size 40
            new ModelSize { ModelId = 15, SizeId = 4, StockQuantity = 6 },  // Size 41
            new ModelSize { ModelId = 15, SizeId = 5, StockQuantity = 10 }, // Size 42

            // New Balance 997 доступные размеры 39, 40, 41, 42
            new ModelSize { ModelId = 16, SizeId = 2, StockQuantity = 10 }, // Size 39
            new ModelSize { ModelId = 16, SizeId = 3, StockQuantity = 4 },  // Size 40
            new ModelSize { ModelId = 16, SizeId = 4, StockQuantity = 0 },  // Size 41 (недоступен)
            new ModelSize { ModelId = 16, SizeId = 5, StockQuantity = 2 },  // Size 42

            // On Cloud доступные размеры 39, 40, 41, 42, 43
            new ModelSize { ModelId = 17, SizeId = 2, StockQuantity = 12 }, // Size 39
            new ModelSize { ModelId = 17, SizeId = 3, StockQuantity = 8 },  // Size 40
            new ModelSize { ModelId = 17, SizeId = 4, StockQuantity = 6 },  // Size 41
            new ModelSize { ModelId = 17, SizeId = 5, StockQuantity = 10 }, // Size 42
            new ModelSize { ModelId = 17, SizeId = 6, StockQuantity = 4 },  // Size 43

            // Nike Blazer доступные размеры 38, 39, 40, 41, 42, 43
            new ModelSize { ModelId = 18, SizeId = 1, StockQuantity = 5 },  // Size 38
            new ModelSize { ModelId = 18, SizeId = 2, StockQuantity = 6 },  // Size 39
            new ModelSize { ModelId = 18, SizeId = 3, StockQuantity = 0 },  // Size 40 (недоступен)
            new ModelSize { ModelId = 18, SizeId = 4, StockQuantity = 7 },  // Size 41
            new ModelSize { ModelId = 18, SizeId = 5, StockQuantity = 2 },  // Size 42
            new ModelSize { ModelId = 18, SizeId = 6, StockQuantity = 0 }  // Size 43 (недоступен)
        );
        await context.SaveChangesAsync();
        }
}
    private static async Task EnsureCommentsExistAsync(ShopApplicationContext context)
    {
        if (!context.Reviews.Any())
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com");

            context.Reviews.AddRange(
                new Review
                {
                    Text = "Great product! Fast shipping!",
                    Rating = 5,
                    Created = DateTime.UtcNow,
                    ProductId = 1,
                    UserId = user.Id
                },
                new Review
                {
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
    
    // не используется
    private static async Task EnsurePhotosExistAsync(ShopApplicationContext context)
    {
        // if (!context.Photos.Any())
        // {
        //     context.Photos.AddRange(
        //         new Photo
        //         {
        //             FileName = "photo1.webp",
        //             FilePath = "/images/photo1.webp",
        //             Length = 2048,
        //             ModelId = 1
        //         },
        //         new Photo
        //         {
        //             FileName = "photo2.webp",
        //             FilePath = "/images/photo2.webp",
        //             Length = 1024,
        //             ModelId = 2
        //         }
        //     );
        //     await context.SaveChangesAsync();
        // }
    }
    private static async Task EnsureAddressesExistAsync(ShopApplicationContext context)
    {
        // if (!context.Addresses.Any())
        // {
        //     var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com");
        //     context.Addresses.AddRange(
        //         new Address
        //         {
        //             Id = 1,
        //             AddressName = "123 Main St",
        //             UserId = user.Id
        //         },
        //         new Address
        //         {
        //             Id = 2,
        //             AddressName = "456 Elm Ave",
        //             UserId = user.Id
        //         }
        //     );
        //     await context.SaveChangesAsync();
        // }
    }
    private static async Task EnsureOrdersExistAsync(ShopApplicationContext context)
    {
        // if (!context.Orders.Any())
        // {
        //     var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com");
        //
        //     // Получаем адрес пользователя для использования в заказах
        //     var address = await context.Addresses.FirstOrDefaultAsync(a => a.UserId == user.Id);
        //
        //     // Извлекаем модели вместе с размерами через ModelSize
        //     var model1 = await context.Models
        //         .Include(m => m.ModelSizes)
        //             .ThenInclude(ms => ms.Size)
        //         .FirstOrDefaultAsync(m => m.ProductId == 1);
        //
        //     var model2 = await context.Models
        //         .Include(m => m.ModelSizes)
        //             .ThenInclude(ms => ms.Size)
        //         .FirstOrDefaultAsync(m => m.ProductId == 2);
        //
        //     if (model1 != null && model2 != null && model1.ModelSizes.Any() && model2.ModelSizes.Any())
        //     {
        //         var size1 = model1.ModelSizes.FirstOrDefault()?.Size;
        //         var size2 = model2.ModelSizes.FirstOrDefault()?.Size;
        //
        //         if (size1 != null && size2 != null && address != null)
        //         {
        //             var orders = new List<Order>
        //             {
        //                 new Order
        //                 {
        //                     UserId = user.Id,
        //                     Created = DateTime.UtcNow,
        //                     Status = OrderStatus.Processed,
        //                     PaymentIntentId = "pi_1GqjYf2eZvKYlo2C8p1JQY1M", // Пример ID платежного намерения
        //                     AddressId = address.Id, // Связываем с адресом
        //                     ContactPhone = "123-456-7890", // Пример контактного телефона
        //                     OrderItems = new List<OrderItem>
        //                     {
        //                         new OrderItem
        //                         {
        //                             ModelId = model1.Id,
        //                             Quantity = 1,
        //                             Amount = model1.Price,
        //                             SizeId = size1.Id
        //                         },
        //                         new OrderItem
        //                         {
        //                             ModelId = model2.Id,
        //                             Quantity = 2,
        //                             Amount = model2.Price,
        //                             SizeId = size2.Id
        //                         }
        //                     }
        //                 }
        //             };
        //
        //             context.Orders.AddRange(orders);
        //             await context.SaveChangesAsync();
        //         }
        //     }
        // }
    }
}
