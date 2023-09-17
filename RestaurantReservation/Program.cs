using RestaurantReservation.DB.EFContext;

using var context = new RestaurantContext();

var isCreated = context.Database.EnsureCreated();

Console.WriteLine(
    isCreated
        ? "The database got created successfully!"
        : "The database have not gotten created!"
    );
