using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Exceptions;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.Repositories;

public class OrdersRepository
{
    private readonly RestaurantContext _restaurantContext;

    public OrdersRepository(RestaurantContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
    }
    
    public async Task AddNewOrder(Order order)
    {
        if (!_restaurantContext.Orders.Any(o => o.OrderDate == order.OrderDate && o.TotalAmount == order.TotalAmount))
        {
            await _restaurantContext.Orders.AddAsync(order);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordExistsException("The order already exists in the database!");
        }
    }

    public async Task DeleteOrder(Order order)
    {
        if (_restaurantContext.Orders.Any(o => o.OrderDate == order.OrderDate && o.TotalAmount == order.TotalAmount))
        {
            _restaurantContext.Orders.Remove(order);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The order does not exist in the database!");
        }
    }

    public async Task UpdateOrder(Order order)
    {
        var existingOrder = await _restaurantContext.Orders.FindAsync(order.OrderId);
        if (existingOrder is not null)
        {
            _restaurantContext.Entry(existingOrder).CurrentValues.SetValues(order);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The order does not exist in the database!");
        }
    }
    
    public List<Order> ListOrdersAndMenuItems(int reservationId)
    {
        return  _restaurantContext.Orders.Where(o => o.ReservationId == reservationId).Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem).ToList();
    }
}