using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DB.EFContext;
using RestaurantReservation.DB.Exceptions;
using RestaurantReservation.DB.Models;

namespace RestaurantReservation.DB.Repositories;

public class ReservationsRepository
{
    private readonly RestaurantContext _restaurantContext;

    public ReservationsRepository(RestaurantContext restaurantContext)
    {
        _restaurantContext = restaurantContext;
    }
    
    public async Task AddNewReservation(Reservation reservation)
    {
        if (_restaurantContext.Reservations.Any(r => r.ReservationDate == reservation.ReservationDate && r.PartySize == reservation.PartySize))
            throw new RecordExistsException("The reservation already exists in the database!");
        
        await _restaurantContext.Reservations.AddAsync(reservation);
        await _restaurantContext.SaveChangesAsync();
        }

    public async Task DeleteReservation(Reservation reservation)
    {
        if (!_restaurantContext.Reservations.Any(r => r.ReservationDate == reservation.ReservationDate && r.PartySize == reservation.PartySize))
            throw new RecordDoesNotExistException("The reservations does not exist in the database!");
        
        _restaurantContext.Reservations.Remove(reservation);
        await _restaurantContext.SaveChangesAsync();
        }

    public async Task UpdateReservation(Reservation reservation)
    {
        var existingReservation = await _restaurantContext.Reservations.FindAsync(reservation.ReservationId);
        
        if (existingReservation is null)
            throw new RecordDoesNotExistException("The reservations does not exist in the database!");
        
        _restaurantContext.Entry(existingReservation).CurrentValues.SetValues(reservation);
        await _restaurantContext.SaveChangesAsync();
        }
    
    public List<Reservation> GetReservationsByCustomer(int customerId)
    {
        return _restaurantContext.Reservations.Where(r => r.CustomerId == customerId).ToList();
    }

    public async Task<List<Reservation>> ListReservationsWithCustomersAndRestaurants()
    {
        const string sql = """
            SELECT ReservationId,
                   ReservationDate,
                   PartySize,
                   CustomerFullName,
                   CustomerEmail,
                   CustomerPhoneNumber,
                   RestaurantName,
                   Address,
                   RestaurantPhoneNumber,
                   OpeningHours
            FROM
                ReservationsWithCustomersAndRestaurants;
        """;

        return await _restaurantContext.Reservations.FromSqlRaw(sql).ToListAsync();
    }
}