using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Exceptions;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.Repositories;

public class RestaurantsRepository
{
    private readonly RestaurantContext _restaurantContext;

    public RestaurantsRepository(RestaurantContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
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
    
    public async Task AddNewRestaurant(Restaurant restaurant)
    {
        if (_restaurantContext.Restaurants.Any(r => r.Name == restaurant.Name && r.PhoneNumber == restaurant.PhoneNumber))
            throw new RecordExistsException("The restaurant already exists in the database!");
            
        await _restaurantContext.Restaurants.AddAsync(restaurant);
        await _restaurantContext.SaveChangesAsync();
        }

    public async Task DeleteRestaurant(Restaurant restaurant)
    {
        if (!_restaurantContext.Restaurants.Any(r => r.Name == restaurant.Name && r.PhoneNumber == restaurant.PhoneNumber))
            throw new RecordDoesNotExistException("The restaurant does not exist in the database!");
        
        _restaurantContext.Restaurants.Remove(restaurant);
        await _restaurantContext.SaveChangesAsync();
        }

    public async Task UpdateRestaurant(Restaurant restaurant)
    {
        var existingRestaurant = await _restaurantContext.Restaurants.FindAsync(restaurant.RestaurantId);
        
        if (existingRestaurant is null)
            throw new RecordDoesNotExistException("The restaurant does not exist in the database!");
        
        _restaurantContext.Entry(existingRestaurant).CurrentValues.SetValues(restaurant);
        await _restaurantContext.SaveChangesAsync();
        }
}