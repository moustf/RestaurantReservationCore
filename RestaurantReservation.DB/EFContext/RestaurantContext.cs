using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext.TypeConverters;
using RestaurantReservation.DB.Models;
using TimeOnlyConverter = RestaurantReservation.DB.EFContext.TypeConverters.TimeOnlyConverter;


namespace RestaurantReservation.DB.EFContext;

public class RestaurantContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Data Source=hazza;Initial Catalog=RestaurantReservationCore;User Id=sa;Password=Root@123;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Restaurant>()
            .Property(e => e.OpeningHours)
            .HasConversion(new TimeOnlyConverter());

        modelBuilder
            .Entity<Reservation>()
            .HasOne(t => t.Customer)
            .WithMany(t => t.Reservations)
            .HasForeignKey(t => t.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Reservation>()
            .HasOne(t => t.Table)
            .WithMany(t => t.Reservations)
            .HasForeignKey(t => t.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Reservation>()
            .HasOne(t => t.Restaurant)
            .WithMany(t => t.Reservations)
            .HasForeignKey(t => t.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Table>()
            .HasOne(t => t.Restaurant)
            .WithMany(t => t.Tables)
            .HasForeignKey(t => t.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Order>()
            .HasOne(t => t.Employee)
            .WithMany(t => t.Orders)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Employee>()
            .HasOne(t => t.Restaurant)
            .WithMany(t => t.Employees)
            .HasForeignKey(t => t.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<MenuItem>()
            .HasOne(t => t.Restaurant)
            .WithMany(t => t.MenuItems)
            .HasForeignKey(t => t.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder
            .Entity<Order>()
            .HasOne(t => t.Reservation)
            .WithMany(t => t.Orders)
            .HasForeignKey(t => t.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder
            .Entity<OrderItem>()
            .HasOne(t => t.Order)
            .WithMany(t => t.OrderItems)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
