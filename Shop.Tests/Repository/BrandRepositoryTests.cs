using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository;

namespace Shop.Tests.Repository
{
    public class BrandRepositoryTests
    {
        private readonly DbContextOptions<ShopApplicationContext> _options;

        public BrandRepositoryTests()
        {
            // Используем уникальное имя базы данных для каждого теста
            _options = new DbContextOptionsBuilder<ShopApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private ShopApplicationContext CreateContext()
        {
            return new ShopApplicationContext(_options);
        }

        [Fact]
        public async Task AddAsync_ReturnsTrue_WhenBrandIsAdded()
        {
            // Arrange
            var brand = new Brand { Name = "New Brand" };
            using (var context = CreateContext())
            {
                var repository = new BrandRepository(context);

                // Act
                var result = await repository.AddAsync(brand);

                // Assert
                Assert.True(result);
                Assert.Equal(1, await context.Brands.CountAsync());
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBrands()
        {
            // Arrange
            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "Brand 1" },
                new Brand { Id = 2, Name = "Brand 2" }
            };
            using (var context = CreateContext())
            {
                await context.Brands.AddRangeAsync(brands);
                await context.SaveChangesAsync();
                var repository = new BrandRepository(context);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                var brandList = Assert.IsAssignableFrom<IEnumerable<Brand>>(result);
                Assert.Equal(2, brandList.Count());
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBrand_WhenBrandExists()
        {
            // Arrange
            var brand = new Brand { Id = 1, Name = "Brand 1" };
            using (var context = CreateContext())
            {
                await context.Brands.AddAsync(brand);
                await context.SaveChangesAsync();
                var repository = new BrandRepository(context);

                // Act
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(brand.Name, result.Name);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenBrandDoesNotExist()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new BrandRepository(context);

                // Act
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenBrandIsUpdated()
        {
            // Arrange
            var brand = new Brand { Id = 1, Name = "Old Brand" };
            using (var context = CreateContext())
            {
                await context.Brands.AddAsync(brand);
                await context.SaveChangesAsync();
                var repository = new BrandRepository(context);

                // Act
                brand.Name = "Updated Brand";
                var result = await repository.UpdateAsync(brand);

                // Assert
                Assert.True(result);
                var updatedBrand = await repository.GetByIdAsync(1);
                Assert.Equal("Updated Brand", updatedBrand.Name);
            }
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenBrandIsDeleted()
        {
            // Arrange
            var brand = new Brand { Id = 1, Name = "Brand to delete" };
            using (var context = CreateContext())
            {
                await context.Brands.AddAsync(brand);
                await context.SaveChangesAsync();
                var repository = new BrandRepository(context);

                // Act
                var result = await repository.DeleteAsync(1);

                // Assert
                Assert.True(result);
                Assert.Equal(0, await context.Brands.CountAsync());
            }
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenBrandDoesNotExist()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new BrandRepository(context);

                // Act
                var result = await repository.DeleteAsync(1);

                // Assert
                Assert.False(result);
                Assert.Equal(0, await context.Brands.CountAsync());
            }
        }
    }
}
