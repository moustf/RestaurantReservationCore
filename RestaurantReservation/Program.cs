using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.InitialData;

var context = new RestaurantContext();
    
try
{
    await context.Database.OpenConnectionAsync();
    
    var seedData = new SeedData();
    await seedData.InitializeDbWithData(context);

    var restaurants = context.Restaurants.ToList();

    foreach (var restaurant in restaurants)
    {
        Console.WriteLine($"{restaurant.RestaurantId} - {restaurant.Name}");
    }

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