using Microsoft.EntityFrameworkCore;

namespace Webshop.Models.DB;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    public static void TestDatabaseConnection(ApplicationDbContext context)
    {
        try
        {
            var version = context.Database.ExecuteSqlRaw("SELECT version()");
            Console.WriteLine("Database connection successful. PostgreSQL version: " + version);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Database connection failed: " + ex.Message);
        }
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public List<Product> Products { get; set; }
}




