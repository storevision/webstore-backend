using System.Security.Cryptography;
using Webshop.App.src.main.Models;
using Webshop.Models;
using Webshop.Models.DB;

namespace Webshop.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void CreateUser(string firstName, string lastName, string email, string? phone, string? address, string password)
    {
        Customer customer = new Customer(firstName, lastName, email, phone, address, password);
        _context.customers.Add(customer);
        _context.SaveChanges();
    }

    public bool Login(string email, string password)
    {
        List<Customer> customers = _context.customers
                .Where(x => x.Email == email)
                .ToList();
        
        Customer customer = customers.FirstOrDefault();
        if (!customer.VerifyPassword(password))
        {
            throw(new Exception("Password verification failed"));
            return false;
        }
        return true;
    }
}