using CloudCustomersDemo.Api.Controllers;
using CloudCustomersDemo.Api.Models;
using CloudCustomersDemo.Api.Services;
using CloudCustomersDemo.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace CloudCustomersDemo.UnitTests.Systems.Controllers;

public class TestUsersController
{
    [Fact]
    public async Task Get_On_Success_ReturnsStatusCode200()
    {
        // Arrange
        //var logger = Mock.Of<ILogger<UsersController>>();
        var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        var logger = factory!.CreateLogger<UsersController>();

        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers());

        var sut = new UsersController(logger, mockUserService.Object);

        // Act
        var result = (OkObjectResult)await sut.Get();

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Get_OnSuccess_InvokesUserService()
    {
        // Given
        var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        var logger = factory!.CreateLogger<UsersController>();

        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(logger, mockUserService.Object);

        // When
        var result = await sut.Get();

        // Then
        mockUserService.Verify(service => service.GetAllUsers(), Times.Once);
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfUsers()
    {
        // Given
        var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        var logger = factory!.CreateLogger<UsersController>();

        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers());

        var sut = new UsersController(logger, mockUserService.Object);

        // When
        var result = await sut.Get();

        // Then
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<User>>();
    }

    [Fact]
    public async Task Get_OnNoUsersFound_Returns404()
    {
        // Given
        var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        var logger = factory!.CreateLogger<UsersController>();

        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(logger, mockUserService.Object);

        // When
        var result = await sut.Get();

        // Then
        result.Should().BeOfType<NotFoundResult>();
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);
    }
}