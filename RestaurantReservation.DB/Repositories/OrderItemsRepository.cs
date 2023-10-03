using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Exceptions;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.Repositories;

public class OrderItemsRepository
{
    private readonly RestaurantContext _restaurantContext;

    public OrderItemsRepository(RestaurantContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
    }
    
    public async Task AddNewOrderItem(OrderItem orderItem)
    {
        if (_restaurantContext.OrderItems.Any(o => o.Quantity == orderItem.Quantity && o.OrderId == orderItem.OrderId && o.ItemId == orderItem.ItemId))
            throw new RecordExistsException("The order item already exists in the database!");
        
        await _restaurantContext.OrderItems.AddAsync(orderItem);
        await _restaurantContext.SaveChangesAsync();
        }

    public async Task DeleteOrderItem(OrderItem orderItem)
    {
        if (!_restaurantContext.OrderItems.Any(o => o.Quantity == orderItem.Quantity && o.OrderId == orderItem.OrderId && o.ItemId == orderItem.ItemId))
            throw new RecordDoesNotExistException("The order items does not exist in the database!");
        
        _restaurantContext.OrderItems.Remove(orderItem);
        await _restaurantContext.SaveChangesAsync();
        }

    public async Task UpdateOrderItem(OrderItem orderItem)
    {
        var existingOrderItem = await _restaurantContext.OrderItems.FindAsync(orderItem.OrderItemId);
        
        if (existingOrderItem is null)
            throw new RecordDoesNotExistException("The order item does not exist in the database!");
        
        _restaurantContext.Entry(existingOrderItem).CurrentValues.SetValues(orderItem);
        await _restaurantContext.SaveChangesAsync();
        }
}