using Webshop.App.src.main.Models;

namespace Webshop.App.src.main.Services;

public interface IUserService
{
    Task<bool> VerifyUserByObjectAsync(TokenUser user);

    public Task CreateUserAsync(User user);
    
    public Task<User?> GetUserByEmailAsync(string email);

    Task<User?>? GetUserByIdAsync(int value);
    Address[] getUserAdresses(int userId);
    void addUserAddress(int userId, Address address);
}