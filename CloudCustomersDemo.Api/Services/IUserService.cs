using CloudCustomersDemo.Api.Models;

namespace CloudCustomersDemo.Api.Services;

public interface IUserService
{
    Task<List<User>> GetAllUsers();
}