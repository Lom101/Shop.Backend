using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.WebAPI.Repository;
using Xunit;

namespace Shop.Tests.Repository
{
    public class ModelRepositoryTests
    {
        private readonly DbContextOptions<ShopApplicationContext> _options;

        public ModelRepositoryTests()
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

        // [Fact]
        // public async Task GetModelByIdAsync_ReturnsModel_WhenModelExists()
        // {
        //     // Arrange
        //     using var context = CreateContext();
        //     var modelRepository = new ModelRepository(context);
        //     var model = new Model { Id = 1, Price = 100, ProductId = 1, ColorId = 1 };
        //     context.Models.Add(model); // Добавляем модель в контекст
        //     await context.SaveChangesAsync(); // Сохраняем изменения
        //
        //     // Act
        //     var result = await modelRepository.GetModelByIdAsync(1);
        //
        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.Equal(1, result.Id);
        //     Assert.Equal(100, result.Price);
        // }

        // [Fact]
        // public async Task GetAllModelsAsync_ReturnsAllModels()
        // {
        //     // Arrange
        //     using var context = CreateContext();
        //     var modelRepository = new ModelRepository(context);
        //     var models = new List<Model>
        //     {
        //         new Model { Id = 1, Price = 100, ProductId = 1, ColorId = 1 },
        //         new Model { Id = 2, Price = 200, ProductId = 2, ColorId = 2 }
        //     };
        //     context.Models.AddRange(models); // Добавляем модели в контекст
        //     await context.SaveChangesAsync(); // Сохраняем изменения
        //
        //     // Act
        //     var result = await modelRepository.GetAllModelsAsync();
        //
        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.Equal(2, result.());
        //     Assert.Equal(1, result.First().Id); // Проверяем первый элемент
        //     Assert.Equal(2, result.Last().Id); // Проверяем второй элемент
        // }

        [Fact]
        public async Task AddModelAsync_ReturnsNewModelId()
        {
            // Arrange
            using var context = CreateContext();
            var modelRepository = new ModelRepository(context);
            var model = new Model { Price = 100, ProductId = 1, ColorId = 1 };

            // Act
            var newModelId = await modelRepository.AddModelAsync(model);

            // Assert
            Assert.True(newModelId > 0);
            var addedModel = await context.Models.FindAsync(newModelId);
            Assert.NotNull(addedModel);
            Assert.Equal(model.Price, addedModel.Price);
        }

        [Fact]
        public async Task UpdateModelAsync_ReturnsTrue_WhenModelExists()
        {
            // Arrange
            using var context = CreateContext();
            var modelRepository = new ModelRepository(context);
            var model = new Model { Id = 1, Price = 100, ProductId = 1, ColorId = 1 };
            context.Models.Add(model); // Добавляем модель в контекст
            await context.SaveChangesAsync(); // Сохраняем изменения

            var updatedModel = new Model { Id = 1, Price = 150, ProductId = 1, ColorId = 1 };

            // Act
            var result = await modelRepository.UpdateModelAsync(updatedModel);

            // Assert
            Assert.True(result);
            var retrievedModel = await context.Models.FindAsync(1);
            Assert.Equal(150, retrievedModel.Price);
        }

        [Fact]
        public async Task DeleteModelAsync_ReturnsTrue_WhenModelExists()
        {
            // Arrange
            using var context = CreateContext();
            var modelRepository = new ModelRepository(context);
            var model = new Model { Id = 1, Price = 100, ProductId = 1, ColorId = 1 };
            context.Models.Add(model); // Добавляем модель в контекст
            await context.SaveChangesAsync(); // Сохраняем изменения

            // Act
            var result = await modelRepository.DeleteModelAsync(1);

            // Assert
            Assert.True(result);
            var deletedModel = await context.Models.FindAsync(1);
            Assert.Null(deletedModel); // Модель должна быть удалена
        }

        [Fact]
        public async Task DeleteModelAsync_ReturnsFalse_WhenModelDoesNotExist()
        {
            // Arrange
            using var context = CreateContext();
            var modelRepository = new ModelRepository(context);

            // Act
            var result = await modelRepository.DeleteModelAsync(999); // Идентификатор несуществующей модели

            // Assert
            Assert.False(result);
        }
    }
}
