using Microsoft.EntityFrameworkCore;
<<<<<<<< HEAD:Models/DB/ApplicationDbContext.cs
========
using Webshop.App.src.main.Models;
using Webshop.Models.Cart;
using Webshop.Models.Products;
>>>>>>>> de11f88 (First DB migration, all necessary models --needs to modify):App/src/main/Data/ApplicationDbContext.cs

namespace Webshop.Models.DB;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
<<<<<<<< HEAD:Models/DB/ApplicationDbContext.cs
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
========
    
    public DbSet<Product> products { get; set; }
    public DbSet<Order> orders { get; set; }
    public DbSet<Customer> customers { get; set; }
    public DbSet<OderDetails> orderDetails { get; set; }
    public DbSet<Payment> payments { get; set; }
    public DbSet<Category> categories { get; set; }
    
    
>>>>>>>> de11f88 (First DB migration, all necessary models --needs to modify):App/src/main/Data/ApplicationDbContext.cs
    
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




