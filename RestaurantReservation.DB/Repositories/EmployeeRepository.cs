using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Exceptions;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.Repositories;

public class EmployeeRepository
{
    private readonly RestaurantContext _restaurantContext;

    public EmployeeRepository(RestaurantContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
    }
    
    public async Task AddNewEmployee(Employee employee)
    {
        if (!_restaurantContext.Employees.Any(e => e.FirstName == employee.FirstName && e.LastName == employee.LastName))
        {
            await _restaurantContext.Employees.AddAsync(employee);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordExistsException("The employee already exists in the database!");
        }
    }
    
    public async Task DeleteEmployee(Employee employee)
    {
        if (_restaurantContext.Employees.Any(e => e.FirstName == employee.FirstName && e.LastName == employee.LastName))
        {
            _restaurantContext.Employees.Remove(employee);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The employee does not exist in the database!");
        }
    }
    
    public async Task UpdateEmployee(Employee employee)
    {
        var existingEmployee = await _restaurantContext.Employees.FindAsync(employee.EmployeeId);
        if (existingEmployee is not null)
        {
            _restaurantContext.Entry(existingEmployee).CurrentValues.SetValues(employee);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The employee does not exist in the database!");
        }
    }
    
    public List<Employee> ListManagers()
    {
        return _restaurantContext.Employees.Where(e => e.Position == "Manager").ToList();
    }
    
    public async Task<decimal> CalculateAverageOrderAmount(int employeeId)
    {
        return await _restaurantContext.Orders.Where(o => o.EmployeeId == employeeId).AverageAsync(o => o.TotalAmount);
    }
}