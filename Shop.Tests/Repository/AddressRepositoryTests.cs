using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository;

namespace Shop.Tests.Repository;

public class AddressRepositoryTests
{
    private readonly DbContextOptions<ShopApplicationContext> _options;

    public AddressRepositoryTests()
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
    public async Task GetByUserIdAsync_ReturnsAddresses_WhenUserHasAddresses()
    {
        // Arrange
        var userId = "user123";
        using (var context = CreateContext())
        {
            var addresses = new List<Address>
            {
                new Address { Id = 1, AddressName = "Main Street 1", UserId = userId },
                new Address { Id = 2, AddressName = "Main Street 2", UserId = userId }
            };
            await context.Addresses.AddRangeAsync(addresses);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var repository = new AddressRepository(context);

            // Act
            var result = await repository.GetByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, address => Assert.Equal(userId, address.UserId));
            Assert.All(result, address => Assert.NotNull(address.AddressName)); // Проверка, что AddressName не null
        }
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsEmpty_WhenUserHasNoAddresses()
    {
        // Arrange
        var userId = "user456";

        using (var context = CreateContext())
        {
            var repository = new AddressRepository(context);

            // Act
            var result = await repository.GetByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}