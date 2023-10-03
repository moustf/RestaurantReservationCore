using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.DB.Migrations
{
    /// <inheritdoc />
    public partial class ListReservatioinWithCustomersAndRestaurants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF NOT EXISTS(
                SELECT 1
                FROM INFORMATION_SCHEMA.VIEWS
                WHERE TABLE_NAME = 'ReservationsWithCustomersAndRestaurants'
                )
                BEGIN
                    DECLARE @sql NVARCHAR(MAX);
                    SET @sql = '
                        CREATE VIEW ReservationsWithCustomersAndRestaurants
                        AS
                        SELECT r.ReservationId,
                               r.ReservationDate,
                               r.PartySize,
                               c.FirstName + '' '' + c.LastName As CustomerFullName,
                               c.Email AS CustomerEmail,
                               c.PhoneNumber AS CustomerPhoneNumber,
                               res.Name AS RestaurantName,
                               res.Address,
                               res.PhoneNumber As RestaurantPhoneNumber,
                               res.OpeningHours
                        FROM
                            Reservations AS r
                                INNER JOIN Customers AS c ON r.CustomerId = c.CustomerId
                                INNER JOIN Restaurants AS res ON r.RestaurantId = res.RestaurantId;
                    ';
                    
                    EXEC sp_executesql @sql;
                END;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS ReservationsWithCustomersAndRestaurants;");
        }
    }
}
