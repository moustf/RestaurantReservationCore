namespace RestaurantReservation.DB.Models;

public class Table
{
    public int TableId { get; set; }
    public int Capacity { get; set; }

    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }

    public List<Reservation> Reservations { get; set; }
}