using Webshop.App.src.main.Models;
using Webshop.Models.DB;

namespace Webshop.App.src.main.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public void CreateUser(string email, string password, string displayName)
    {
        User user = new User(email, password, displayName);
        _context.users.Add(user);
        _context.SaveChanges();
    }

    public bool Login(string email, string password)
    {
        List<User> customers = _context.users
                .Where(x => x.Email == email)
                .ToList();
        
        User user = customers.FirstOrDefault();
        if (user != null && !user.VerifyPassword(password))
        {
            throw(new Exception("Password verification failed"));
            return false;
        }
        return true;
    }
}