using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shop.DTO;
using Shop.Entities;
using Shop.Interfaces;
using Shop.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Shop.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ShopApplicationContext _context;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<ShopApplicationContext>()
                .UseInMemoryDatabase(databaseName: "ShopTestDb")
                .Options;
            _context = new ShopApplicationContext(options);

            _mapperMock = new Mock<IMapper>();

            _userService = new UserService(_context, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "John Doe" },
                new User { Id = 2, Name = "Jane Doe" }
            };

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            var userDTOs = new List<UserDTO>
            {
                new UserDTO { Id = 1, Name = "John Doe" },
                new UserDTO { Id = 2, Name = "Jane Doe" }
            };

            _mapperMock.Setup(m => m.Map<IEnumerable<UserDTO>>(It.IsAny<IEnumerable<User>>()))
                       .Returns(userDTOs);

            // Act  
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            
            // Clean up
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync(); // Удаление пользователя из контекста
        }

        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John Doe" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var userDTO = new UserDTO { Id = 1, Name = "John Doe" };

            _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                       .Returns(userDTO);

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDTO.Id, result.Id);
            
            // Clean up
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(); // Удаление пользователя из контекста
        }

        [Fact]
        public async Task AddUserAsync_AddsUser_ReturnsUserDTO()
        {
            // Arrange
            var userDTO = new UserDTO { Name = "John Doe" };
            var user = new User { Id = 1, Name = "John Doe" };

            _mapperMock.Setup(m => m.Map<User>(It.IsAny<UserDTO>()))
                       .Returns(user);
            _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                       .Returns(userDTO);

            // Act
            var result = await _userService.AddUserAsync(userDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDTO.Name, result.Name);
            Assert.Equal(1, _context.Users.Count());
            
            // Clean up
            _context.Users.Remove(user); ///////////////////////////////////////////////////////////////////
            await _context.SaveChangesAsync(); // Удаление пользователя из контекста
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John Doe" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var updatedUserDto = new UserDTO { Id = 1, Name = "Jane Doe" };
            
            // Настройка мока для преобразования UserDTO в User
            _mapperMock.Setup(m => m.Map(It.IsAny<UserDTO>(), It.IsAny<User>()))
                .Callback<UserDTO, User>((dto, u) => u.Name = dto.Name);

            // Act
            await _userService.UpdateUserAsync(updatedUserDto);
            await _context.SaveChangesAsync();

            // Assert
            var updatedUser = await _context.Users.FindAsync(1);
            if (updatedUser == null) throw new ArgumentNullException(nameof(updatedUser));
            Assert.Equal(updatedUserDto.Name, updatedUser.Name);
    
            // Clean up
            _context.Users.Remove(updatedUser);
            await _context.SaveChangesAsync(); // Удаление пользователя из контекста
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John Doe" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            await _userService.DeleteUserAsync(1);

            // Assert
            Assert.Null(await _context.Users.FindAsync(1));
        }

        [Fact]
        public async Task UserExistsAsync_UserExists_ReturnsTrue()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John Doe" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.UserExistsAsync(1);

            // Assert
            Assert.True(result);
            
            // Clean up
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(); // Удаление пользователя из контекста
        }

        [Fact]
        public async Task UserExistsAsync_UserDoesNotExist_ReturnsFalse()
        {
            // Act
            var result = await _userService.UserExistsAsync(1);

            // Assert
            Assert.False(result);
        }
    }
}
