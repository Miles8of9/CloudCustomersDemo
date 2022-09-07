using CloudCustomersDemo.Api.Config;
using CloudCustomersDemo.Api.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace CloudCustomersDemo.Api.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly UsersApiOptions _apiConfig;

    public UserService(HttpClient httpClient, IOptions<UsersApiOptions> apiConfig)
    {
        _httpClient = httpClient;
        _apiConfig = apiConfig.Value;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var userResponse = await _httpClient.GetAsync(_apiConfig.Endpoint);

        if (userResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return new List<User>();
        }

        var responseContent = userResponse.Content;
        var allUsers = await responseContent.ReadFromJsonAsync<List<User>>();

        return allUsers!.ToList();
    }
}