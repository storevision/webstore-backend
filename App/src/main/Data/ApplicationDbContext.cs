using Microsoft.EntityFrameworkCore;
using Webshop.App.src.main.Models;
using Webshop.Models.Cart;
using Webshop.Models.Products;

namespace Webshop.Models.DB;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Product> products { get; set; }
    public DbSet<Order> orders { get; set; }
    public DbSet<User> users { get; set; }
    public DbSet<OderDetails> orderDetails { get; set; }
    public DbSet<Payment> payments { get; set; }
    public DbSet<Category> categories { get; set; }
    public DbSet<Inventory> inventory { get; set; }
    public DbSet<Address> adresses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
    
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