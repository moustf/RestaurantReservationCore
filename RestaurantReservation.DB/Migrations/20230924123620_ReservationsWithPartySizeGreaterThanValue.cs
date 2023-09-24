using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.DB.Migrations
{
    /// <inheritdoc />
    public partial class ReservationsWithPartySizeGreaterThanValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.procedures
                    WHERE name = 'sp_CustomerReservationsWithPartySizeGreaterThan'
                )
                BEGIN
                    DECLARE @sql NVARCHAR(MAX);

                    SET @sql = '
                        CREATE PROCEDURE sp_CustomerReservationsWithPartySizeGreaterThan (@partySize INT)
                        AS
                        BEGIN
                            SELECT c.CustomerId,
                                   c.FirstName,
                                   c.LastName,
                                   c.Email,
                                   c.PhoneNumber
                            FROM Customers AS c
                            INNER JOIN Reservations AS r ON c.CustomerId = r.CustomerId
                            WHERE r.PartySize > @partySize; -- Corrected the condition here
                        END;
                    ';

                    EXEC sp_executesql @sql;
                END;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS sp_CustomerReservationsWithPartySizeGreaterThan");
        }
    }
}
