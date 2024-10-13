using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository;
using Xunit;

namespace Shop.Tests.Repository
{
    public class CategoryRepositoryTests
    {
        private readonly DbContextOptions<ShopApplicationContext> _options;

        public CategoryRepositoryTests()
        {
            // Use a unique database name for each test to avoid conflicts
            _options = new DbContextOptionsBuilder<ShopApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private ShopApplicationContext CreateContext()
        {
            return new ShopApplicationContext(_options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoryRepository(context);
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" }
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCategory_WhenExists()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoryRepository(context);
            var category = new Category { Id = 1, Name = "Category1" };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Name, result?.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoryRepository(context);

            // Act
            var result = await repository.GetByIdAsync(999); // Non-existent ID

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByNameAsync_ReturnsCategory_WhenExists()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoryRepository(context);
            var category = new Category { Id = 1, Name = "Category1" };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByNameAsync("Category1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result?.Id);
        }

        [Fact]
        public async Task GetByNameAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoryRepository(context);

            // Act
            var result = await repository.GetByNameAsync("NonExistentCategory");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsCategory_ReturnsTrue()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoryRepository(context);
            var category = new Category { Name = "NewCategory" };

            // Act
            var result = await repository.AddAsync(category);

            // Assert
            Assert.True(result);
            Assert.NotNull(await repository.GetByNameAsync("NewCategory"));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCategory_ReturnsTrue()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoryRepository(context);
            var category = new Category { Id = 1, Name = "OldCategory" };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            category.Name = "UpdatedCategory";

            // Act
            var result = await repository.UpdateAsync(category);

            // Assert
            Assert.True(result);
            var updatedCategory = await repository.GetByIdAsync(1);
            Assert.Equal("UpdatedCategory", updatedCategory?.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesCategory_ReturnsTrue()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoryRepository(context);
            var category = new Category { Id = 1, Name = "CategoryToDelete" };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.DeleteAsync(1);

            // Assert
            Assert.True(result);
            Assert.Null(await repository.GetByIdAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotExists()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoryRepository(context);

            // Act
            var result = await repository.DeleteAsync(999); // Non-existent ID

            // Assert
            Assert.False(result);
        }
    }
}
