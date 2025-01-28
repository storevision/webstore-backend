using Microsoft.EntityFrameworkCore;
using Webshop.App.src.main.Models;

namespace Webshop.Models.DB;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Product> products { get; set; }
    public DbSet<User> users { get; set; }
    public DbSet<Order> orders { get; set; }
    public DbSet<OrderDetails> orderDetails { get; set; }
    public DbSet<Payment> payments { get; set; }
    public DbSet<Category> categories { get; set; }
    public DbSet<Inventory> inventory { get; set; }
    public DbSet<Address> addresses { get; set; }
    public DbSet<Cart> carts { get; set; }
    public DbSet<Review> reviews { get; set; }
    public DbSet<ProductRating> productRatings { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .EnableSensitiveDataLogging()
            .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<Cart>()
            .HasKey(e => new { e.UserId, e.ProductId });
        
        modelBuilder.Entity<OrderDetails>()
            .HasKey(o=> new {o.OrderId, o.ProductId});
        
        modelBuilder.Entity<Review>()
            .HasKey(r => new { r.ProductId, r.CustomerId });
        
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductRating>(entity =>
        {
            entity.HasNoKey();
            entity.ToTable("product_ratings", t => t.ExcludeFromMigrations());
        });
        
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