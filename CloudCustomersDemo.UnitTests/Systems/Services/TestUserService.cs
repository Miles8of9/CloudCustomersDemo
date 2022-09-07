using CloudCustomersDemo.Api.Config;
using CloudCustomersDemo.Api.Models;
using CloudCustomersDemo.Api.Services;
using CloudCustomersDemo.UnitTests.Fixtures;
using CloudCustomersDemo.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace CloudCustomersDemo.UnitTests.Systems.Services;

public class TestUserService
{
    [Fact]
    public async Task GetAllUsers_WhenCalled_InvokesHttpGetRequest()
    {
        // Given
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "https://www.example.com/users";
        var handlerMock = MockHttpMessageHandler<User>
            .SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);

        var config = Options.Create(
            new UsersApiOptions { Endpoint = endpoint }
        );

        var sut = new UserService(httpClient, config);

        // When
        var users = await sut.GetAllUsers();

        // Then
        handlerMock
            .Protected()
            .Verify(
                "SendAsync", 
                Times.Exactly(1), 
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Fact]
    public async Task GetAllUsers_WhenHits404_ReturnsEmptyListOfUsers()
    {
        // Given
        var handlerMock = MockHttpMessageHandler<User>.SetupReturn404();
        var endpoint = "https://www.example.com/users";
        var httpClient = new HttpClient(handlerMock.Object);

        var config = Options.Create(
            new UsersApiOptions { Endpoint = endpoint }
        );

        var sut = new UserService(httpClient, config);

        // When
        var result = await sut.GetAllUsers();

        // Then
        result.Count.Should().Be(0);

        handlerMock
        .Protected()
        .Verify(
            "SendAsync", 
            Times.Exactly(1), 
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task GetAllUsers_WhenCalled_ReturnsListOfUsers()
    {
        // Given
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "https://www.example.com/users";
        var handlerMock = MockHttpMessageHandler<User>
            .SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);

        var config = Options.Create(
            new UsersApiOptions { Endpoint = endpoint }
        );

        var sut = new UserService(httpClient, config);

        // When
        var result = await sut.GetAllUsers();

        // Then
        result.Should().BeOfType<List<User>>();
    }

    [Fact]
    public async Task GetAllUsers_WhenCalled_ReturnsListOfUsersOfExpectedSize()
    {
        // Given
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "https://www.example.com/users";
        var handlerMock = MockHttpMessageHandler<User>
            .SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);

        var config = Options.Create(
            new UsersApiOptions { Endpoint = endpoint }
        );

        var sut = new UserService(httpClient, config);

        // When
        var result = await sut.GetAllUsers();

        // Then
        result.Count.Should().Be(expectedResponse.Count);
    }

    [Fact]
    public async Task GetAllUsers_WhenCalled_InvokesConfiguredExternalUrl()
    {
        // Given
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "https://www.example.com/users";
        var handlerMock = MockHttpMessageHandler<User>
            .SetupBasicGetResourceList(expectedResponse, endpoint);

        var httpClient = new HttpClient(handlerMock.Object);

        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UserService(httpClient, config);

        // When
        var result = await sut.GetAllUsers();

        // Then
        //result.Count.Should().Be(expectedResponse.Count);

        handlerMock
            .Protected()
            .Verify(
                "SendAsync", 
                Times.Exactly(1), 
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get &&
                                                    req.RequestUri!.ToString() == endpoint),
                ItExpr.IsAny<CancellationToken>()
            );
    }
}