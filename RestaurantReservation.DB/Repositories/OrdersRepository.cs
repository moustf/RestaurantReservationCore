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
        if (_restaurantContext.Orders.Any(o => o.OrderDate == order.OrderDate && o.TotalAmount == order.TotalAmount))
            throw new RecordExistsException("The order already exists in the database!");
        
        await _restaurantContext.Orders.AddAsync(order);
        await _restaurantContext.SaveChangesAsync();
        }

    public async Task DeleteOrder(Order order)
    {
        if (!_restaurantContext.Orders.Any(o => o.OrderDate == order.OrderDate && o.TotalAmount == order.TotalAmount))
            throw new RecordDoesNotExistException("The order does not exist in the database!");
        
        _restaurantContext.Orders.Remove(order);
        await _restaurantContext.SaveChangesAsync();
        
    }

    public async Task UpdateOrder(Order order)
    {
        var existingOrder = await _restaurantContext.Orders.FindAsync(order.OrderId);
        
        if (existingOrder is null)
            throw new RecordDoesNotExistException("The order does not exist in the database!");
        
        _restaurantContext.Entry(existingOrder).CurrentValues.SetValues(order);
        await _restaurantContext.SaveChangesAsync();
        }
    
    public List<Order> ListOrdersAndMenuItems(int reservationId)
    {
        return  _restaurantContext.Orders.Where(o => o.ReservationId == reservationId).Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem).ToList();
    }
}