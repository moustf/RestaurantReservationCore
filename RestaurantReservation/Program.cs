using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Exceptions;
using RestaurantReservation.DB.InitialData;
using RestaurantReservation.DB.Models;
using RestaurantReservation.DB.Repositories;

var context = new RestaurantContext();
var ordersRepo = new OrdersRepository(context);
var customersRepo = new CustomersRepository(context);
var restaurantsRepo = new RestaurantsRepository(context);
var employeesRepo = new EmployeeRepository(context);
var menuItemsRepo = new MenuItemsRepository(context);
var tablesRepo = new TablesRepository(context);
var reservationsRepo = new ReservationsRepository(context);

try
{
    await context.Database.OpenConnectionAsync();

    var initializeData = new SeedData();
    await initializeData.InitializeDbWithData(context);

    // await CrudOperateOnEntities(context);
    await QueryEntities(context);
}
catch (RecordExistsException e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e);
}
catch (RecordDoesNotExistException e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
finally
{
    await context.Database.CloseConnectionAsync();
}

async Task CrudOperateOnEntities(RestaurantContext context)
{

    #region Customer

    // ::: Adding new Entity; Begin.
    
    await customersRepo.AddNewCustomer(new Customer()
    {
        FirstName = "Mohammed",
        LastName = "Salem",
        Email = "test@test.com",
        PhoneNumber = "123456789",
    });
    var mohammedSalem = await context.Customers.FirstOrDefaultAsync(c => c.Email == "test@test.com");
    
    Console.WriteLine($"Customer created: {mohammedSalem?.CustomerId} - {mohammedSalem?.FirstName}, {mohammedSalem?.LastName}.");
    
    // ::: Adding new Entity; End.

    // ::: Deleting Entity; Begin.
    await customersRepo.DeleteCustomer(await context.Customers.FirstAsync(c => c.Email == "test@test.com"));
    var deletedMohSalem = await context.Customers.FirstOrDefaultAsync(c => c.Email == "test@test.com");
    
    Console.WriteLine(deletedMohSalem is null ? "Mohammed Salem got deleted successfully!" : "ERROR while deleting the customer!");
    
    // ::: Deleting Entity; End.
    
    // ::: Updating Entity; Begin.

    var initialCustomer = new Customer()
    {
        FirstName = "Mohammed",
        LastName = "Salem",
        Email = "test@test.com",
        PhoneNumber = "123456789",
    };
    var customerToUpdateWith = new Customer()
    {
        FirstName = "Rami",
        LastName = "Kamal",
        Email = "rami@kamal.com",
        PhoneNumber = "987654321",
    };
    
    await customersRepo.AddNewCustomer(initialCustomer);
    var customerBeforeUpdate = await context.Customers.FirstOrDefaultAsync(c => c.Email == "test@test.com");
    customerToUpdateWith.CustomerId = customerBeforeUpdate!.CustomerId;
    
    Console.WriteLine($"Customer before update: {customerBeforeUpdate?.CustomerId} - {customerBeforeUpdate?.FirstName}, {customerBeforeUpdate?.LastName}.");

    await customersRepo.UpdateCustomer(customerToUpdateWith);
    var updatedCustomer = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerToUpdateWith.CustomerId);
    
    Console.WriteLine($"Customer after updating: {updatedCustomer?.CustomerId} - {updatedCustomer?.FirstName}, {updatedCustomer?.LastName}.");

    // ::: Updating Entity; End.
    
    // Cleaning up.
    
    Console.WriteLine("Customers region clean up!");
    
    await customersRepo.DeleteCustomer(updatedCustomer!);
    Console.WriteLine("Customer got deleted successfully!");

    #endregion

    #region Restaurant
    
    // ::: Adding new Entity; Begin.

    await restaurantsRepo.AddNewRestaurant(new Restaurant()
    {
        Name = "Restaurant One",
        Address = "Gaza, Palestine",
        PhoneNumber = "11223344",
        OpeningHours = new TimeOnly(),
    });
    
    var resOne = await context.Restaurants.FirstOrDefaultAsync(c => c.PhoneNumber == "11223344");
    
    Console.WriteLine($"Restaurant created: {resOne!.RestaurantId} - {resOne!.Name}, {resOne!.Address}.");

    // ::: Adding new Entity; End.

    // ::: Deleting Entity; Begin.
    
    await restaurantsRepo.DeleteRestaurant(await context.Restaurants.FirstAsync(c => c.PhoneNumber == "11223344"));
    var deletedRes = await context.Restaurants.FirstOrDefaultAsync(c => c.PhoneNumber == "11223344");
    
    Console.WriteLine(deletedRes is null ? "Restaurant one got deleted successfully!" : "ERROR while deleting the restaurant!");

    // ::: Deleting Entity; End.

    // ::: Updating Entity; Begin.
    
    var initialRestaurant = new Restaurant()
    {
        Name = "Restaurant Two",
        Address = "Gaza, Palestine",
        PhoneNumber = "22334455",
        OpeningHours = new TimeOnly(),
    };
    var restaurantToUpdateWith = new Restaurant()
    {
        Name = "Restaurant Three",
        Address = "Gaza, Palestine",
        PhoneNumber = "33445566",
        OpeningHours = new TimeOnly(),
    };
    
    await restaurantsRepo.AddNewRestaurant(initialRestaurant);
    var restaurantBeforeUpdate = await context.Restaurants.FirstOrDefaultAsync(r => r.PhoneNumber == "22334455");
    restaurantToUpdateWith.RestaurantId = restaurantBeforeUpdate!.RestaurantId;
    
    Console.WriteLine($"Restaurant before update: {restaurantBeforeUpdate?.RestaurantId} - {restaurantBeforeUpdate?.Name}, {restaurantBeforeUpdate?.OpeningHours}.");

    await restaurantsRepo.UpdateRestaurant(restaurantToUpdateWith);
    var updatedRestaurant = await context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == restaurantToUpdateWith.RestaurantId);
    
    Console.WriteLine($"Restaurant after updating: {updatedRestaurant?.RestaurantId} - {updatedRestaurant?.Name}, {updatedRestaurant?.Address}.");

    // ::: Updating Entity; End.

    // Cleaning up.
    
    Console.WriteLine("Restaurants region clean up!");
    
    await restaurantsRepo.DeleteRestaurant(await context.Restaurants.FirstAsync(c => c.RestaurantId == restaurantToUpdateWith.RestaurantId));
    Console.WriteLine("Restaurant in the restaurants region got deleted successfully!");

    #endregion
    
    #region Employee
    
    // ::: Adding new Entity; Begin.

    await restaurantsRepo.AddNewRestaurant(new Restaurant()
    {
        Name = "Restaurant Three",
        Address = "Gaza, Palestine",
        PhoneNumber = "999888776",
        OpeningHours = new TimeOnly(),
    });

    var restaurantThree = await context.Restaurants.FirstOrDefaultAsync(r => r.PhoneNumber == "999888776");
    
    await employeesRepo.AddNewEmployee(new Employee()
    {
        FirstName = "Mohammed",
        LastName = "Salem",
        Position = "Manager",
        RestaurantId = restaurantThree!.RestaurantId,
    });
    
    var empOne = await context.Employees.FirstOrDefaultAsync(e => e.FirstName == "Mohammed" && e.LastName == "Salem");
    
    Console.WriteLine($"Employee created: {empOne!.EmployeeId} - {empOne!.FirstName}, {empOne!.LastName}.");
    
    // ::: Adding new Entity; End.
    
    // ::: Deleting Entity; Begin.
    
    await employeesRepo.DeleteEmployee(await context.Employees.FirstAsync(e => e.FirstName == "Mohammed" && e.LastName == "Salem"));
    var deletedEmp = await context.Employees.FirstOrDefaultAsync(e => e.FirstName == "Mohammed" && e.LastName == "Salem");
    
    Console.WriteLine(deletedEmp is null ? "Employee one got deleted successfully!" : "ERROR while deleting the employee!");
    
    // ::: Deleting Entity; End.
    
    // ::: Updating Entity; Begin.
    
    var initialEmployee = new Employee()
    {
        FirstName = "Rami",
        LastName = "Salah",
        Position = "Jnior",
        RestaurantId = restaurantThree.RestaurantId,
    };
    var employeeToUpdateWith = new Employee()
    {
        FirstName = "Hani",
        LastName = "Kamal",
        Position = "Manager",
        RestaurantId = restaurantThree.RestaurantId,
    };
    
    await employeesRepo.AddNewEmployee(initialEmployee);
    var employeeBeforeUpdate = await context.Employees.FirstOrDefaultAsync(e => e.FirstName == "Rami" && e.LastName == "Salah");
    employeeToUpdateWith.EmployeeId = employeeBeforeUpdate!.EmployeeId;
    
    Console.WriteLine($"Employee before update: {employeeBeforeUpdate?.EmployeeId} - {employeeBeforeUpdate?.FirstName}, {employeeBeforeUpdate?.LastName}.");

    await employeesRepo.UpdateEmployee(employeeToUpdateWith);
    var updatedEmployee = await context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeToUpdateWith.EmployeeId);
    
    Console.WriteLine($"Employee after updating: {updatedEmployee?.EmployeeId} - {updatedEmployee?.FirstName}, {updatedEmployee?.LastName}.");

    
    // ::: Updating Entity; End.
    
    // Cleaning up.
    
    Console.WriteLine("Employees region clean up!");
    
    await restaurantsRepo.DeleteRestaurant(await context.Restaurants.FirstAsync(r => r.RestaurantId == restaurantThree!.RestaurantId));
    Console.WriteLine("Restaurant in the employees region got Deleted!");
    
    
    #endregion
    
    #region MenuItem
    
    // ::: Adding new Entity; Begin.
    
    await restaurantsRepo.AddNewRestaurant(new Restaurant()
    {
        Name = "Restaurant four",
        Address = "Gaza, Palestine",
        PhoneNumber = "88776666",
        OpeningHours = new TimeOnly(),
    });

    var restaurantFour = await context.Restaurants.FirstOrDefaultAsync(r => r.PhoneNumber == "88776666");
    
    await menuItemsRepo.AddNewMenuItem(new MenuItem()
    {
        Name = "Coffee Machine",
        Description = "Coffee grinding machine",
        Price = 455.99M,
        RestaurantId = restaurantFour!.RestaurantId,
    });
    
    var coffeeMachine = await context.MenuItems.FirstOrDefaultAsync(m => m.Name == "Coffee Machine" && m.Description == "Coffee grinding machine");
    
    Console.WriteLine($"Menu item created: {coffeeMachine!.ItemId} - {coffeeMachine!.Name}, {coffeeMachine!.Description}.");
    
    // ::: Adding new Entity; End.
    
    // ::: Deleting Entity; Begin.
    
    await menuItemsRepo.DeleteMenuItem(await context.MenuItems.FirstAsync(m => m.Name == "Coffee Machine" && m.Description == "Coffee grinding machine"));
    var deletedMenuItem = await context.MenuItems.FirstOrDefaultAsync(m => m.Name == "Coffee Machine" && m.Description == "Coffee grinding machine");
    
    Console.WriteLine(deletedMenuItem is null ? "Menu item one got deleted successfully!" : "ERROR while deleting the menu item!");
    
    // ::: Deleting Entity; End.
    
    // ::: Updating Entity; Begin.
    
    var initialMenuItem = new MenuItem()
    {
        Name = "Tea",
        Description = "Green tea",
        Price = 15.55M,
        RestaurantId = restaurantFour.RestaurantId,
    };
    var menuItemToUpdateWith = new MenuItem()
    {
        Name = "Sugar",
        Description = "White sugar",
        Price = 9.55M,
        RestaurantId = restaurantFour.RestaurantId,
    };
    
    await menuItemsRepo.AddNewMenuItem(initialMenuItem);
    var menuItemBeforeUpdate = await context.MenuItems.FirstOrDefaultAsync(m => m.Name == "Tea" && m.Description == "Green tea");
    menuItemToUpdateWith.ItemId = menuItemBeforeUpdate!.ItemId;
    
    Console.WriteLine($"Menu item before update: {menuItemBeforeUpdate?.ItemId} - {menuItemBeforeUpdate?.Name}, {menuItemBeforeUpdate?.Description}.");

    await menuItemsRepo.UpdateMenuItem(menuItemToUpdateWith);
    var updatedMenuItem = await context.MenuItems.FirstOrDefaultAsync(m => m.Name == "Sugar" && m.Description == "White sugar");
    
    Console.WriteLine($"Menu item after updating: {updatedMenuItem?.ItemId} - {updatedMenuItem?.Name}, {updatedMenuItem?.Description}.");

    // ::: Updating Entity; End.
    
    // Cleaning up.
    
    Console.WriteLine("Menu items region clean up!");
    
    await restaurantsRepo.DeleteRestaurant(await context.Restaurants.FirstAsync(r => r.RestaurantId == restaurantFour.RestaurantId));
    Console.WriteLine("Restaurant in the menu item region got deleted successfully!");
    
    
    #endregion
    
    #region Table
    
    // ::: Adding new Entity; Begin.
    
    await restaurantsRepo.AddNewRestaurant(new Restaurant()
    {
        Name = "Restaurant Five",
        Address = "Gaza, Palestine",
        PhoneNumber = "222333444333",
        OpeningHours = new TimeOnly(),
    });

    var restaurantFive = await context.Restaurants.FirstOrDefaultAsync(r => r.PhoneNumber == "222333444333");
    
    await tablesRepo.AddNewTable(new Table()
    {
        Capacity = 8,
        RestaurantId = restaurantFive!.RestaurantId,
    });
    
    var table = await context.Tables.FirstOrDefaultAsync(t => t.Capacity == 8 && t.RestaurantId == restaurantFive!.RestaurantId);
    
    Console.WriteLine($"Menu item created: {table!.TableId} - {table!.Capacity}.");
    
    // ::: Adding new Entity; End.
    
    // ::: Deleting Entity; Begin.
    
    await tablesRepo.DeleteTable(await context.Tables.FirstAsync(t => t.TableId == table.TableId));
    var deletedTable = await context.Tables.FirstOrDefaultAsync(t => t.TableId == table.TableId);
    
    Console.WriteLine(deletedTable is null ? "Table one got deleted successfully!" : "ERROR while deleting the table!");
    
    // ::: Deleting Entity; End.
    
    // ::: Updating Entity; Begin.
    
    var initialTable = new Table()
    {
        Capacity = 5,
        RestaurantId = restaurantFive!.RestaurantId,
    };
    var tableToUpdateWith = new Table()
    {
        Capacity = 7,
        RestaurantId = restaurantFive!.RestaurantId,
    };
    
    await tablesRepo.AddNewTable(initialTable);
    var tableBeforeUpdate = await context.Tables.FirstOrDefaultAsync(t => t.Capacity == 5 && t.RestaurantId == restaurantFive!.RestaurantId);
    tableToUpdateWith.TableId = tableBeforeUpdate!.TableId;
    
    Console.WriteLine($"Table before update: {tableBeforeUpdate?.TableId} - {tableBeforeUpdate?.Capacity}.");

    await tablesRepo.UpdateTable(tableToUpdateWith);
    var updatedTable = await context.Tables.FirstOrDefaultAsync(t => t.TableId == tableToUpdateWith.TableId);
    
    Console.WriteLine($"Table after updating: {tableBeforeUpdate?.TableId} - {tableBeforeUpdate?.Capacity}.");
    
    // ::: Updating Entity; End.
    
    // Cleaning up.
    
    Console.WriteLine("Tables region clean up!");
    
    await restaurantsRepo.DeleteRestaurant(await context.Restaurants.FirstAsync(r => r.RestaurantId == restaurantFive.RestaurantId));
    Console.WriteLine("Restaurant in the tables region got deleted successfully!");
    
    #endregion
    
    #region Reservations
    
    // ::: Adding new Entity; Begin.

    await customersRepo.AddNewCustomer(new Customer()
    {
        FirstName = "Salama",
        LastName = "Hamada",
        Email = "salamahamada22@gamil.com",
        PhoneNumber = "111222333",
    });
    var customerOne = await context.Customers.FirstOrDefaultAsync(c => c.Email == "salamahamada22@gamil.com");
    await restaurantsRepo.AddNewRestaurant(new Restaurant()
    {
        Name = "Restaurant Six",
        Address = "Gaza, Palestine",
        PhoneNumber = "98778999",
        OpeningHours = new TimeOnly(),
    });
    var restaurantSix = await context.Restaurants.FirstOrDefaultAsync(r => r.PhoneNumber == "98778999");
    await tablesRepo.AddNewTable(new Table()
    {
        Capacity = 4,
        RestaurantId = restaurantSix!.RestaurantId,
    });
    var tableTwo = await context.Tables.FirstOrDefaultAsync(t =>
        t.Capacity == 4 && t.RestaurantId == restaurantSix!.RestaurantId);

    var dateOne = DateTime.Now;
    await reservationsRepo.AddNewReservation(new Reservation()
    {
        ReservationDate = dateOne,
        PartySize = 12,
        CustomerId = customerOne!.CustomerId,
        RestaurantId = restaurantSix.RestaurantId,
        TableId = tableTwo!.TableId,
    });
    var reservationOne = await context.Reservations.FirstOrDefaultAsync(r => r.ReservationDate == dateOne && r.PartySize == 12);
    
    Console.WriteLine($"Reservation created: {reservationOne!.ReservationId} - {reservationOne!.ReservationDate}.");

    // ::: Adding new Entity; End.

    // ::: Deleting Entity; Begin.
    
    await reservationsRepo.DeleteReservation(await context.Reservations.FirstAsync(r => r.ReservationId == reservationOne.ReservationId));
    var deletedReservation = await context.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservationOne.ReservationId);
    
    Console.WriteLine(deletedReservation is null ? "Reservation got deleted successfully!" : "ERROR while deleting the table!");

    // ::: Deleting Entity; End.

    // ::: Updating Entity; Begin.

    var dateTwo = DateTime.Now.Add(new TimeSpan((12)));
    var dateThree = DateTime.Now.Add(new TimeSpan((24)));
    var initialReservation = new Reservation()
    {
        ReservationDate = dateTwo,
        PartySize = 10,
        CustomerId = customerOne!.CustomerId,
        RestaurantId = restaurantSix.RestaurantId,
        TableId = tableTwo!.TableId,
    };
    var reservationToUpdateWith = new Reservation()
    {
        ReservationDate = dateThree,
        PartySize = 19,
        CustomerId = customerOne!.CustomerId,
        RestaurantId = restaurantSix.RestaurantId,
        TableId = tableTwo!.TableId,
    };
    
    await reservationsRepo.AddNewReservation(initialReservation);
    var reservationBeforeUpdate = await context.Reservations.FirstOrDefaultAsync(r => r.ReservationDate == dateTwo && r.PartySize == 10);
    reservationToUpdateWith.ReservationId = reservationBeforeUpdate!.ReservationId;
    
    Console.WriteLine($"Reservation before update: {reservationBeforeUpdate?.ReservationId} - {reservationBeforeUpdate?.ReservationDate}.");

    await reservationsRepo.UpdateReservation(reservationToUpdateWith);
    var updatedReservation = await context.Reservations.FirstOrDefaultAsync(r => r.ReservationDate == dateThree && r.PartySize == 19);
    
    Console.WriteLine($"Reservation after updating: {updatedReservation?.ReservationId} - {updatedReservation?.ReservationDate}.");

    // ::: Updating Entity; End.

    // Cleaning up.
    
    Console.WriteLine("Reservations region clean up!");

    await reservationsRepo.DeleteReservation(
        await context.Reservations.FirstAsync(r => r.ReservationId == reservationToUpdateWith.ReservationId));
    Console.WriteLine("Reservation in the reservations region got deleted successfully!");
    await customersRepo.DeleteCustomer(await context.Customers.FirstAsync(c => c.CustomerId == customerOne.CustomerId));
    Console.WriteLine("Customer in the reservations region got deleted successfully!");
    await restaurantsRepo.DeleteRestaurant(
        await context.Restaurants.FirstAsync(r => r.RestaurantId == restaurantSix.RestaurantId));
    Console.WriteLine("Restaurant in the reservations region got deleted successfully!");

    #endregion
    
    #region Orders
    
    // ::: Adding new Entity; Begin.
    
    await customersRepo.AddNewCustomer(new Customer()
    {
        FirstName = "Jemmy",
        LastName = "Maccce",
        Email = "jemmmmy55@gamil.com",
        PhoneNumber = "223334445",
    });
    var customerTwo = await context.Customers.FirstOrDefaultAsync(c => c.Email == "jemmmmy55@gamil.com");
    await restaurantsRepo.AddNewRestaurant(new Restaurant()
    {
        Name = "Restaurant Seven",
        Address = "Gaza, Palestine",
        PhoneNumber = "12344321",
        OpeningHours = new TimeOnly(),
    });
    var restaurantSeven = await context.Restaurants.FirstOrDefaultAsync(r => r.PhoneNumber == "12344321");
    await tablesRepo.AddNewTable(new Table()
    {
        Capacity = 87,
        RestaurantId = restaurantSeven!.RestaurantId,
    });
    var tableThree = await context.Tables.FirstOrDefaultAsync(t =>
        t.Capacity == 87 && t.RestaurantId == restaurantSeven!.RestaurantId);

    var dateFour = DateTime.Now.Add(new TimeSpan(22));
    await reservationsRepo.AddNewReservation(new Reservation()
    {
        ReservationDate = dateFour,
        PartySize = 18,
        CustomerId = customerTwo!.CustomerId,
        RestaurantId = restaurantSeven.RestaurantId,
        TableId = tableThree!.TableId,
    });
    var reservationTwo = await context.Reservations.FirstOrDefaultAsync(r => r.ReservationDate == dateFour && r.PartySize == 18);
    await employeesRepo.AddNewEmployee(new Employee()
    {
        FirstName = "Ramaa",
        LastName = "Jomaa",
        Position = "Manager",
        RestaurantId = restaurantSeven.RestaurantId,
    });
    var employeeOne = await context.Employees.FirstOrDefaultAsync(e => e.FirstName == "Ramaa" && e.LastName == "Jomaa");
    await ordersRepo.AddNewOrder(new Order()
    {
        OrderDate = dateFour,
        TotalAmount = 6677.99M,
        ReservationId = reservationTwo!.ReservationId,
        EmployeeId = employeeOne!.EmployeeId,
    });
    var orderOne = await context.Orders.FirstOrDefaultAsync(o => o.OrderDate == dateFour && o.TotalAmount == 6677.99M);
    
    Console.WriteLine($"Order created: {orderOne!.OrderId} - {orderOne!.OrderDate} - {orderOne!.TotalAmount}.");

    // ::: Adding new Entity; End.

    // ::: Deleting Entity; Begin.
    
    await ordersRepo.DeleteOrder(await context.Orders.FirstAsync(o => o.OrderId == orderOne.OrderId));
    var deletedOrder = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderOne.OrderId);
    
    Console.WriteLine(deletedOrder is null ? "Order got deleted successfully!" : "ERROR while deleting the order!");

    // ::: Deleting Entity; End.

    // ::: Updating Entity; Begin.

    var dateFive = DateTime.Now.Add(new TimeSpan(15));
    var dateSix = DateTime.Now.Add(new TimeSpan(14));
    var initialOrder = new Order()
    {
        OrderDate = dateFive,
        TotalAmount = 5678.99M,
        ReservationId = reservationTwo!.ReservationId,
        EmployeeId = employeeOne!.EmployeeId,
    };
    var orderToUpdateWith = new Order()
    {
        OrderDate = dateSix,
        TotalAmount = 4567.555M,
        ReservationId = reservationTwo!.ReservationId,
        EmployeeId = employeeOne!.EmployeeId,
    };
    
    await ordersRepo.AddNewOrder(initialOrder);
    var orderBeforeUpdate = await context.Orders.FirstOrDefaultAsync(o => o.OrderDate == dateFive && o.TotalAmount == 5678.99M);
    orderToUpdateWith!.OrderId = orderBeforeUpdate!.OrderId;
    
    Console.WriteLine($"Reservation before update: {orderBeforeUpdate!.OrderId} - {orderBeforeUpdate!.OrderDate} - {orderBeforeUpdate!.TotalAmount}.");
    
    await ordersRepo.UpdateOrder(orderToUpdateWith);
    var updatedOrder = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderToUpdateWith.OrderId);
    
    Console.WriteLine($"Reservation after update: {updatedOrder!.OrderId} - {updatedOrder!.OrderDate} - {updatedOrder!.TotalAmount}.");

    // ::: Updating Entity; End.

    // Cleaning up.
    
    Console.WriteLine("Orders region clean up!");

    await customersRepo.DeleteCustomer(await context.Customers.FirstAsync(c => c.CustomerId == customerTwo!.CustomerId));
    Console.WriteLine("Customer in the orders region got deleted successfully!");
    await ordersRepo.DeleteOrder(
        await context.Orders.FirstAsync(o => o.OrderId == updatedOrder.OrderId));
    Console.WriteLine("Order in the orders region got deleted successfully!");
    await reservationsRepo.DeleteReservation(await context.Reservations.FirstAsync(r => r.ReservationId == reservationTwo.ReservationId));
    Console.WriteLine("Reservations in the orders region got deleted successfully!");
    await restaurantsRepo.DeleteRestaurant(
        await context.Restaurants.FirstAsync(r => r.RestaurantId == restaurantSeven!.RestaurantId));
    Console.WriteLine("Restaurant in the orders region got deleted successfully!");
    await employeesRepo.DeleteEmployee(
        await context.Employees.FirstAsync(e => e.EmployeeId == employeeOne!.EmployeeId));
    Console.WriteLine("Employee in the orders region got deleted successfully!");

    #endregion
}

async Task QueryEntities(RestaurantContext context)
{
    var managers = employeesRepo.ListManagers();
    var reservations = reservationsRepo.GetReservationsByCustomer(1);
    var orderAndItems = ordersRepo.ListOrdersAndMenuItems(1);
    var menuItems = menuItemsRepo.ListOrderedMenuItems(1);
    var amountAverage = await employeesRepo.CalculateAverageOrderAmount(1);
    var restaurantRevenue = await restaurantsRepo.CalculateRestaurantRevenue(1);
    var customers = await customersRepo.CustomerReservationsWithPartySizeGreaterThan(5);
    
    Console.WriteLine($"The total amount for the employee: {amountAverage}");

    Console.WriteLine($"Restaurant revenue is: {restaurantRevenue}");
    
    foreach (var manager in managers)
    {
        Console.WriteLine($"{manager.EmployeeId} - {manager.FirstName} {manager.LastName} - {manager.Position}.");
    }
    
    foreach (var reservation in reservations)
    {
        Console.WriteLine($"{reservation.ReservationId} - {reservation.ReservationDate} {reservation.PartySize}.");
    }
    
    foreach (var order in orderAndItems)
    {
        Console.WriteLine($"Order: {order.OrderId} - {order.OrderDate} {order.TotalAmount}.");
        if (order.OrderItems.Count > 0) Console.WriteLine("The items are: ");
        foreach (var orderItem in order.OrderItems)
        {
            Console.WriteLine($"Item: {orderItem.MenuItem.ItemId} - {orderItem.MenuItem.Name} - {orderItem.MenuItem.Description} - {orderItem.MenuItem.Price}.");
        }
    }
    
    foreach (var menuItem in menuItems)
    {
        Console.WriteLine($"Item: {menuItem.ItemId} - {menuItem.Name} - {menuItem.Description} - {menuItem.Price}.");
    }

    foreach (var customer in customers)
    {
        Console.WriteLine($"The customer: {customer.CustomerId} - {customer.FirstName} {customer.LastName} - {customer.Email} - {customer.PhoneNumber}");
    }
}
