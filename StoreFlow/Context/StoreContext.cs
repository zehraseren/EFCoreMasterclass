using StoreFlow.Entities;
using Microsoft.EntityFrameworkCore;

namespace StoreFlow.Context;

public class StoreContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=Zehra;Initial Catalog=StoreFlowDb;Integrated Security=true;TrustServerCertificate=true");
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .Property(c => c.CustomerBalance)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Product>()
            .Property(p => p.ProductPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
}
