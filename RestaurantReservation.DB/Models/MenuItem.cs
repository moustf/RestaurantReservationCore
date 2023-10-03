using Microsoft.EntityFrameworkCore;

namespace RestaurantReservation.DB.Models;

[PrimaryKey(nameof(ItemId))]
public class MenuItem
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }

    public List<OrderItem> OrderItems { get; set; }
}