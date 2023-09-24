using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Exceptions;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.Repositories;

public class TablesRepository
{
    private readonly RestaurantContext _restaurantContext;

    public TablesRepository(RestaurantContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
    }
    
    public async Task AddNewTable(Table table)
    {
        if (!_restaurantContext.Tables.Any(t => t.Capacity == table.Capacity && t.RestaurantId == table.RestaurantId))
        {
            await _restaurantContext.Tables.AddAsync(table);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordExistsException("The table already exists in the database!");
        }
    }

    public async Task DeleteTable(Table table)
    {
        if (_restaurantContext.Tables.Any(t => t.Capacity == table.Capacity && t.RestaurantId == table.RestaurantId))
        {
            _restaurantContext.Tables.Remove(table);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The table does not exist in the database!");
        }
    }

    public async Task UpdateTable(Table table)
    {
        var existingTable = await _restaurantContext.Tables.FindAsync(table.TableId);
        if (existingTable is not null)
        {
            _restaurantContext.Entry(existingTable).CurrentValues.SetValues(table);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The table does not exist in the database!");
        }
    }
}