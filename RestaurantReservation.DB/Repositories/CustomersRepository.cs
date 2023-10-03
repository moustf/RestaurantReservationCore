using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Exceptions;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.Repositories;

public class CustomersRepository
{
    private readonly RestaurantContext _restaurantContext;

    public CustomersRepository(RestaurantContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
    }

    public async Task AddNewCustomer(Customer customer)
    {
        if (_restaurantContext.Customers.Any(c => c.Email == customer.Email))
            throw new RecordExistsException("The user already exists in the database!");

        await _restaurantContext.Customers.AddAsync(customer);
        await _restaurantContext.SaveChangesAsync();
    }

    public async Task DeleteCustomer(Customer customer)
    {
        if (_restaurantContext.Customers.Any(c => c.Email == customer.Email))
            throw new RecordDoesNotExistException("The user does not exist in the database!");
            
        _restaurantContext.Customers.Remove(customer);
        await _restaurantContext.SaveChangesAsync();
    }

    public async Task UpdateCustomer(Customer customer)
    {
        var existingCustomer = await _restaurantContext.Customers.FindAsync(customer.CustomerId);
        
        if (existingCustomer is null)
            throw new RecordDoesNotExistException("The user does not exist in the database!");
        
        _restaurantContext.Entry(existingCustomer).CurrentValues.SetValues(customer);
        await _restaurantContext.SaveChangesAsync();
        }

    public async Task<List<Customer>> CustomerReservationsWithPartySizeGreaterThan(int partySize)
    {
        const string sql = "EXEC sp_CustomerReservationsWithPartySizeGreaterThan @partySizeParam;";
        var parameters = new SqlParameter("@partySizeParam", SqlDbType.Int)
        {
            Value = partySize
        };

        return await _restaurantContext.Customers.FromSqlRaw(sql, parameters).ToListAsync();
    }
}