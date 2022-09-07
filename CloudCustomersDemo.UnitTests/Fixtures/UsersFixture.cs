using CloudCustomersDemo.Api.Models;

namespace CloudCustomersDemo.UnitTests.Fixtures;

public static class UsersFixture
{
    public static List<User> GetTestUsers() => new()
    {
        new User
        {
            Id = 1,
            Name = "Mary Jane",
            Email = "mary.jane@example.com",
            Address = new Address()
            {
                Street = "123 Main St",
                City = "Madison",
                ZipCode = "53704"
            }
        },
        new User
        {
            Id = 2,
            Name = "Peter Parker",
            Email = "peter.parker@example.com",
            Address = new Address()
            {
                Street = "123 Market St",
                City = "New York",
                ZipCode = "213124"
            }
        }
    };
}