using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.InitialData;

public class SeedData
{
    public async Task InitializeDbWithData(RestaurantContext context)
    {
        if (!context.Customers.Any())
        {
            var customers = new List<Customer>();
            
            for (var m = 1; m <= 10; m++)
            {
                customers.Add(
                        new Customer()
                        {
                            FirstName = m == 1
                                ? $"Micheal the {m}st"
                                : m == 2
                                    ? $"Micheal the {m}nd"
                                    : m == 3
                                        ? $"Micheal the {m}rd"
                                        : $"Micheal the {m}th",
                            LastName = "Schmeichel",
                            Email = $"micheal.{m}@dummy.com",
                            PhoneNumber = $"{m}{m}{m}123456789",
                        }
                    );
            }

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        if (!context.Restaurants.Any())
        {
            var restaurants = new List<Restaurant>();

            for (var m = 1; m <= 10; m++)
            {
                restaurants.Add(
                        new Restaurant()
                        {
                            Name = $"Restaurant {m}",
                            Address = "Palestine, Gaza, Capital Mall",
                            PhoneNumber = $"{m}{m}{m}123456789",
                            OpeningHours = new TimeOnly(),
                        }
                    );
            }

            await context.Restaurants.AddRangeAsync(restaurants);
            await context.SaveChangesAsync();
        }
        
        if (!context.Employees.Any())
        {
            var employees = new List<Employee>();
            var restaurants = context.Restaurants.ToList();

            for (var m = 1; m <= 10; m++)
            {
                employees.Add(
                        new Employee()
                        {
                            FirstName = m == 1
                                ? $"Mohammed the {m}st"
                                : m == 2
                                    ? $"Mohammed the {m}nd"
                                    : m == 3
                                        ? $"Mohammed the {m}rd"
                                        : $"Mohammed the {m}th",
                            LastName = "Sallam",
                            Position = m >= 1 && m < 5
                                ? "Employee"
                                : "Manager",
                            RestaurantId = restaurants[m - 1].RestaurantId,
                        }
                    );
            }

            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }
        
        if (!context.MenuItems.Any())
        {
            var menuItems = new List<MenuItem>();
            var restaurants = context.Restaurants.ToList();

            for (var m = 1; m <= 10; m++)
            {
                menuItems.Add(
                        new MenuItem()
                        {
                            Name = $"Menu Item {m}",
                            Description = $"Menu Item Number {m}",
                            Price = m * 100.555M,
                            RestaurantId = restaurants[m - 1].RestaurantId,
                        }
                    );
            }

            await context.MenuItems.AddRangeAsync(menuItems);
            await context.SaveChangesAsync();
        }
        
        if (!context.Tables.Any())
        {
            var tables = new List<Table>();
            var restaurants = context.Restaurants.ToList();

            for (var m = 1; m <= 10; m++)
            {
                tables.Add(
                    new Table()
                    {
                        Capacity = m * 10,
                        RestaurantId = restaurants[m - 1].RestaurantId,
                    }
                );
            }

            await context.Tables.AddRangeAsync(tables);
            await context.SaveChangesAsync();
        }
        
        if (!context.Reservations.Any())
        {
            var reservations = new List<Reservation>();
            var customers = context.Customers.ToList();
            var restaurants = context.Restaurants.ToList();
            var tables = context.Tables.ToList();

            for (var m = 1; m <= 10; m++)
            {
                reservations.Add(
                    new Reservation()
                    {
                        ReservationDate = new DateTime(),
                        PartySize = m * 5,
                        CustomerId = customers[m - 1].CustomerId,
                        RestaurantId = restaurants[m - 1].RestaurantId,
                        TableId = tables[m - 1].TableId,
                    }
                );
            }

            await context.Reservations.AddRangeAsync(reservations);
            await context.SaveChangesAsync();
        }
        
        if (!context.Orders.Any())
        {
            var orders = new List<Order>();
            var employees = context.Employees.ToList();
            var reservations = context.Reservations.ToList();

            for (var m = 1; m <= 10; m++)
            {
                orders.Add(
                        new Order()
                        {
                            OrderDate = new DateTime(),
                            TotalAmount = m * 500.00M,
                            ReservationId = reservations[m - 1].ReservationId,
                            EmployeeId = employees[m - 1].EmployeeId,
                        }
                    );
            }

            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();
        }
        
        if (!context.OrderItems.Any())
        {
            var orderItems = new List<OrderItem>();
            var orders = context.Orders.ToList();
            var menuItems = context.MenuItems.ToList();

            for (var m = 1; m <= 10; m++)
            {
                orderItems.Add(
                        new OrderItem()
                        {
                            Quantity = m * 10,
                            OrderId = orders[m - 1].OrderId,
                            ItemId = menuItems[m - 1].ItemId,
                            MenuItem = menuItems[m - 1],
                        }
                    );
            }

            await context.OrderItems.AddRangeAsync(orderItems);
            await context.SaveChangesAsync();
        }
    }
}