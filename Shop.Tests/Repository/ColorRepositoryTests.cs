using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.Tests.Repository
{
    public class ColorRepositoryTests
    {
        private readonly DbContextOptions<ShopApplicationContext> _options;

        public ColorRepositoryTests()
        {
            // Use a unique database name for each test to ensure isolation.
            _options = new DbContextOptionsBuilder<ShopApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private ShopApplicationContext CreateContext()
        {
            return new ShopApplicationContext(_options);
        }

        private IColorRepository CreateRepository(ShopApplicationContext context)
        {
            return new ColorRepository(context);
        }

        [Fact]
        public async Task AddAsync_AddsColor_WhenColorIsValid()
        {
            // Arrange
            using var context = CreateContext();
            var repository = CreateRepository(context);
            var color = new Color { Name = "Red" };

            // Act
            var result = await repository.AddAsync(color);
            var addedColor = await context.Colors.FirstOrDefaultAsync(c => c.Name == "Red");

            // Assert
            Assert.True(result);
            Assert.NotNull(addedColor);
            Assert.Equal("Red", addedColor?.Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllColors()
        {
            // Arrange
            using var context = CreateContext();
            var repository = CreateRepository(context);
            context.Colors.AddRange(
                new Color { Name = "Red" },
                new Color { Name = "Blue" }
            );
            await context.SaveChangesAsync();

            // Act
            var colors = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(colors);
            Assert.Equal(2, colors.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsColor_WhenColorExists()
        {
            // Arrange
            using var context = CreateContext();
            var repository = CreateRepository(context);
            var color = new Color { Name = "Green" };
            context.Colors.Add(color);
            await context.SaveChangesAsync();

            // Act
            var retrievedColor = await repository.GetByIdAsync(color.Id);

            // Assert
            Assert.NotNull(retrievedColor);
            Assert.Equal(color.Name, retrievedColor?.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenColorDoesNotExist()
        {
            // Arrange
            using var context = CreateContext();
            var repository = CreateRepository(context);

            // Act
            var retrievedColor = await repository.GetByIdAsync(999);

            // Assert
            Assert.Null(retrievedColor);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesColor_WhenColorExists()
        {
            // Arrange
            using var context = CreateContext();
            var repository = CreateRepository(context);
            var color = new Color { Name = "Yellow" };
            context.Colors.Add(color);
            await context.SaveChangesAsync();
            color.Name = "Orange";

            // Act
            var result = await repository.UpdateAsync(color);
            var updatedColor = await context.Colors.FindAsync(color.Id);

            // Assert
            Assert.True(result);
            Assert.NotNull(updatedColor);
            Assert.Equal("Orange", updatedColor?.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesColor_WhenColorExists()
        {
            // Arrange
            using var context = CreateContext();
            var repository = CreateRepository(context);
            var color = new Color { Name = "Purple" };
            context.Colors.Add(color);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.DeleteAsync(color.Id);
            var deletedColor = await context.Colors.FindAsync(color.Id);

            // Assert
            Assert.True(result);
            Assert.Null(deletedColor);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenColorDoesNotExist()
        {
            // Arrange
            using var context = CreateContext();
            var repository = CreateRepository(context);

            // Act
            var result = await repository.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}
