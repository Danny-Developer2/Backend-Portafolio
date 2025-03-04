using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using API.Entities;
using API.DTOs;
using API.Interfaces;
using API.DTO;

public class UsersControllerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _controller = new UsersController(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUserById_UserExists_ReturnsOkResult()
    {
        // Arrange
        var user = new AppUser { Id = 1, Name = "TestUser" };
        _userRepositoryMock.Setup(repo => repo.GetUserById(1)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetUserById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUser = Assert.IsType<AppUser>(okResult.Value);
        Assert.Equal(1, returnedUser.Id);
    }

    [Fact]
    public async Task GetUserById_UserDoesNotExist_ReturnsNotFound()
    {
         
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetUserById(1)).ReturnsAsync((AppUser)new AppUser { Id = 1, Name = "TestUser" });
        



        // Act
        var result = await _controller.GetUserById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result); // Corrected to NotFoundResult (not NotFoundObjectResult)
    }

    [Fact]
    public async Task CreateUser_ValidUser_ReturnsCreatedAtAction()
    {
        // Arrange
        var userDto = new UserDTO { Id = 1, Name = "NewUser" };
        var user = new AppUser { Id = 1, Name = "NewUser" };

        _userRepositoryMock.Setup(repo => repo.AddUser(userDto)).ReturnsAsync(user);

        // Act
        var result = await _controller.CreateUser(userDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedUser = Assert.IsType<AppUser>(createdResult.Value);
        Assert.Equal(1, returnedUser.Id);
    }

    [Fact]
    public async Task UpdateUser_UserExists_ReturnsOk()
    {
        // Arrange
        var userDto = new UserDTO { Id = 1, Name = "UpdatedUser" };
        var user = new AppUser { Id = 1, Name = "UpdatedUser" };

        _userRepositoryMock.Setup(repo => repo.UpdateUser(1, userDto)).ReturnsAsync(user);

        // Act
        var result = await _controller.UpdateUser(1, userDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<AppUser>(okResult.Value);
        Assert.Equal("UpdatedUser", returnedUser.Name);
    }

    [Fact]
    public async Task UpdateUser_UserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var userDto = new UserDTO { Id = 1, Name = "UpdatedUser" };
        _userRepositoryMock.Setup(repo => repo.UpdateUser(1, userDto)).ReturnsAsync((AppUser)new AppUser { Id = 1, Name = "TestUser" });

        // Act
        var result = await _controller.UpdateUser(1, userDto);

        // Assert
        Assert.IsType<NotFoundResult>(result); // Corrected to NotFoundResult
    }

    [Fact]
    public async Task DeleteUser_UserExists_ReturnsNoContent()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.DeleteUser(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteUser(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteUser_UserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.DeleteUser(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteUser(1);

        // Assert
        Assert.IsType<NotFoundResult>(result); // Corrected to NotFoundResult
    }
}
