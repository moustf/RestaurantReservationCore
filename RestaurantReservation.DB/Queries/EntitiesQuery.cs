using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.Queries;

public class EntitiesQuery
{
    private readonly RestaurantContext _restaurantContext;

    public EntitiesQuery(RestaurantContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
    }

    public List<Employee> ListManagers()
    {
        return _restaurantContext.Employees.Where(e => e.Position == "Manager").ToList();
    }

    public List<Reservation> GetReservationsByCustomer(int customerId)
    {
        return _restaurantContext.Reservations.Where(r => r.CustomerId == customerId).ToList();
    }

    public List<Order> ListOrdersAndMenuItems(int reservationId)
    {
        return  _restaurantContext.Orders.Where(o => o.ReservationId == reservationId).Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem).ToList();
    }

    public List<MenuItem> ListOrderedMenuItems(int reservationId)
    {
        return _restaurantContext.Orders.Where(o => o.ReservationId == reservationId).Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem).SelectMany(o => o.OrderItems.Select(oi => oi.MenuItem)).OrderBy(mi => mi.ItemId).ToList();
    }

    public async Task<decimal> CalculateAverageOrderAmount(int employeeId)
    {
        return await _restaurantContext.Orders.Where(o => o.EmployeeId == employeeId).AverageAsync(o => o.TotalAmount);
    }

    public async Task<decimal> CalculateRestaurantRevenue(int restaurantId)
    {
        return (decimal) await _restaurantContext.Database.ExecuteSqlRawAsync(
            """
                DECLARE @revenue DECIMAL(12, 4);
                EXEC @revenue = dbo.fn_RestaurantTotalRevenue @restaurant_id = {0};
                
                SELECT @revenue
            """,
            restaurantId
        );
    }
}