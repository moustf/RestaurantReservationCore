using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Exceptions;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.Queries;

public class EntitiesCrudOperate
{
    private readonly RestaurantContext _restaurantContext;

    public EntitiesCrudOperate(RestaurantContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
    }
    
    #region Customer

    public async Task AddNewCustomer(Customer customer)
    {
        if (!_restaurantContext.Customers.Any(c => c.Email == customer.Email))
        {
            await _restaurantContext.Customers.AddAsync(customer);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordExistsException("The user already exists in the database!");
        }
    }

    public async Task DeleteCustomer(Customer customer)
    {
        if (_restaurantContext.Customers.Any(c => c.Email == customer.Email))
        {
            _restaurantContext.Customers.Remove(customer);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The user does not exist in the database!");
        }
    }

    public async Task UpdateCustomer(Customer customer)
    {
        var existingCustomer = await _restaurantContext.Customers.FindAsync(customer.CustomerId);
        if (existingCustomer is not null)
        {
            _restaurantContext.Entry(existingCustomer).CurrentValues.SetValues(customer);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The user does not exist in the database!");
        }
    }
    
    #endregion
    
    #region Restaurant

    public async Task AddNewRestaurant(Restaurant restaurant)
    {
        if (!_restaurantContext.Restaurants.Any(r => r.PhoneNumber == restaurant.PhoneNumber))
        {
            await _restaurantContext.Restaurants.AddAsync(restaurant);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordExistsException("The restaurant already exists in the database!");
        }
    }

    public async Task DeleteRestaurant(Restaurant restaurant)
    {
        if (_restaurantContext.Restaurants.Any(r => r.PhoneNumber == restaurant.PhoneNumber))
        {
            _restaurantContext.Restaurants.Remove(restaurant);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The restaurant does not exist in the database!");
        }
    }

    public async Task UpdateRestaurant(Restaurant restaurant)
    {
        var existingRestaurant = await _restaurantContext.Restaurants.FindAsync(restaurant.RestaurantId);
        if (existingRestaurant is not null)
        {
            _restaurantContext.Entry(existingRestaurant).CurrentValues.SetValues(restaurant);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The restaurant does not exist in the database!");
        }
    }
    
    #endregion

    #region Employees

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

    #endregion
    
    #region MneuItems

    public async Task AddNewMenuItem(MenuItem menuItem)
    {
        if (!_restaurantContext.MenuItems.Any(m => m.Name == menuItem.Name && m.Description == menuItem.Description))
        {
            await _restaurantContext.MenuItems.AddAsync(menuItem);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordExistsException("The menu item already exists in the database!");
        }
    }

    public async Task DeleteMenuItem(MenuItem menuItem)
    {
        if (_restaurantContext.MenuItems.Any(m => m.Name == menuItem.Name && m.Description == menuItem.Description))
        {
            _restaurantContext.MenuItems.Remove(menuItem);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The menu item does not exist in the database!");
        }
    }

    public async Task UpdateMenuItem(MenuItem menuItem)
    {
        var existingMenuItem = await _restaurantContext.MenuItems.FindAsync(menuItem.ItemId);
        if (existingMenuItem is not null)
        {
            _restaurantContext.Entry(existingMenuItem).CurrentValues.SetValues(menuItem);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The menu item does not exist in the database!");
        }
    }

    #endregion

    #region Table

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

    #endregion

    #region Reservations

    public async Task AddNewReservation(Reservation reservation)
    {
        if (!_restaurantContext.Reservations.Any(r => r.ReservationDate == reservation.ReservationDate && r.PartySize == reservation.PartySize))
        {
            await _restaurantContext.Reservations.AddAsync(reservation);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordExistsException("The reservation already exists in the database!");
        }
    }

    public async Task DeleteReservation(Reservation reservation)
    {
        if (_restaurantContext.Reservations.Any(r => r.ReservationDate == reservation.ReservationDate && r.PartySize == reservation.PartySize))
        {
            _restaurantContext.Reservations.Remove(reservation);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The reservations does not exist in the database!");
        }
    }

    public async Task UpdateReservation(Reservation reservation)
    {
        var existingReservation = await _restaurantContext.Reservations.FindAsync(reservation.ReservationId);
        if (existingReservation is not null)
        {
            _restaurantContext.Entry(existingReservation).CurrentValues.SetValues(reservation);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The reservations does not exist in the database!");
        }
    }

    #endregion
    
    #region Orders
    
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
    
    #endregion
    
    #region OrderItems
    
    public async Task AddNewOrderItem(OrderItem orderItem)
    {
        if (!_restaurantContext.OrderItems.Any(o => o.Quantity == orderItem.Quantity && o.OrderId == orderItem.OrderId && o.ItemId == orderItem.ItemId))
        {
            await _restaurantContext.OrderItems.AddAsync(orderItem);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordExistsException("The order item already exists in the database!");
        }
    }

    public async Task DeleteOrderItem(OrderItem orderItem)
    {
        if (_restaurantContext.OrderItems.Any(o => o.Quantity == orderItem.Quantity && o.OrderId == orderItem.OrderId && o.ItemId == orderItem.ItemId))
        {
            _restaurantContext.OrderItems.Remove(orderItem);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The order items does not exist in the database!");
        }
    }

    public async Task UpdateOrderItem(OrderItem orderItem)
    {
        var existingOrderItem = await _restaurantContext.OrderItems.FindAsync(orderItem.OrderItemId);
        if (existingOrderItem is not null)
        {
            _restaurantContext.Entry(existingOrderItem).CurrentValues.SetValues(orderItem);
            await _restaurantContext.SaveChangesAsync();
        }
        else
        {
            throw new RecordDoesNotExistException("The order item does not exist in the database!");
        }
    }
    
    #endregion
}