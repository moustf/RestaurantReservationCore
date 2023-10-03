using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Exceptions;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.Repositories;

public class MenuItemsRepository
{
    private readonly RestaurantContext _restaurantContext;

    public MenuItemsRepository(RestaurantContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
    }
    
    public async Task AddNewMenuItem(MenuItem menuItem)
    {
        if (_restaurantContext.MenuItems.Any(m => m.Name == menuItem.Name && m.Description == menuItem.Description))
            throw new RecordExistsException("The menu item already exists in the database!");
            
        await _restaurantContext.MenuItems.AddAsync(menuItem);
        await _restaurantContext.SaveChangesAsync();  
        }

    public async Task DeleteMenuItem(MenuItem menuItem)
    {
        if (!_restaurantContext.MenuItems.Any(m => m.Name == menuItem.Name && m.Description == menuItem.Description))
            throw new RecordDoesNotExistException("The menu item does not exist in the database!");
        
        _restaurantContext.MenuItems.Remove(menuItem);
        await _restaurantContext.SaveChangesAsync();
        }

    public async Task UpdateMenuItem(MenuItem menuItem)
    {
        var existingMenuItem = await _restaurantContext.MenuItems.FindAsync(menuItem.ItemId);
        
        if (existingMenuItem is null)
            throw new RecordDoesNotExistException("The menu item does not exist in the database!");
        
        _restaurantContext.Entry(existingMenuItem).CurrentValues.SetValues(menuItem);
        await _restaurantContext.SaveChangesAsync();
        }
    
    public List<MenuItem> ListOrderedMenuItems(int reservationId)
    {
        return _restaurantContext.Orders.Where(o => o.ReservationId == reservationId).Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem).SelectMany(o => o.OrderItems.Select(oi => oi.MenuItem)).OrderBy(mi => mi.ItemId).ToList();
    }
}