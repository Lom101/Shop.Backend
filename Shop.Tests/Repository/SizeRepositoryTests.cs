using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository;

namespace Shop.Tests.Repository;

public class SizeRepositoryTests
{
    private readonly DbContextOptions<ShopApplicationContext> _options;

    public SizeRepositoryTests()
    {
        // Используем уникальное имя базы данных для каждого теста для обеспечения изоляции.
        _options = new DbContextOptionsBuilder<ShopApplicationContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private ShopApplicationContext CreateContext()
    {
        return new ShopApplicationContext(_options);
    }

    [Fact]
    public async Task GetAllSizesAsync_ShouldReturnAllSizes()
    {
        // Arrange
        using var context = CreateContext();
        context.Sizes.AddRange(new List<Size>
        {
            new Size { Id = 1, Name = "Small" },
            new Size { Id = 2, Name = "Medium" }
        });
        await context.SaveChangesAsync();

        var repository = new SizeRepository(context);

        // Act
        var sizes = await repository.GetAllSizesAsync();

        // Assert
        Assert.Equal(2, sizes.Count());
    }

    [Fact]
    public async Task GetSizeByIdAsync_ShouldReturnSize_WhenSizeExists()
    {
        // Arrange
        using var context = CreateContext();
        context.Sizes.Add(new Size { Id = 1, Name = "Large" });
        await context.SaveChangesAsync();

        var repository = new SizeRepository(context);

        // Act
        var size = await repository.GetSizeByIdAsync(1);

        // Assert
        Assert.NotNull(size);
        Assert.Equal("Large", size.Name);
    }

    [Fact]
    public async Task GetSizeByIdAsync_ShouldReturnNull_WhenSizeDoesNotExist()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new SizeRepository(context);

        // Act
        var size = await repository.GetSizeByIdAsync(1);

        // Assert
        Assert.Null(size);
    }
}