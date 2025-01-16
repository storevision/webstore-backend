using Webshop.App.src.main.Models;
using Webshop.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Webshop.App.src.main.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private IUserService _userServiceImplementation;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Asynchrones Erstellen eines Benutzers
    public async Task CreateUserAsync(User user)
    {
        _context.users.Add(user);
        await _context.SaveChangesAsync();
    }

    // Benutzer anhand der E-Mail abrufen
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.users.FirstOrDefaultAsync(u => u.CustomerID == id);
    }

    // Benutzeranmeldung validieren
    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);

        if (user == null || !user.VerifyPassword(password))
        {
            return false;
        }

        return true;
    }

    // Verifizieren eines Benutzers basierend auf dem JWT-Token-Objekt
    public async Task<bool> VerifyUserByObjectAsync(TokenUser tokenUser)
    {
        if (string.IsNullOrEmpty(tokenUser.Id) || string.IsNullOrEmpty(tokenUser.Email))
        {
            return false;
        }

        if (!int.TryParse(tokenUser.Id, out var userId))
        {
            return false;
        }

        var user = await _context.users.FirstOrDefaultAsync(u => u.CustomerID == userId && u.Email == tokenUser.Email);

        return user != null;
    }
    
    public Address[] getUserAdresses(int userId)
    {
        List<Address> addressList = new List<Address>();
        Address[] addresses = _context.addresses.FromSqlRaw("SELECT * FROM addresses WHERE user_id = {0}", userId).ToArray();
        foreach (var address in addresses)
        {
            addressList.Add(address);
        }
        
        return addressList.ToArray();
    }
}