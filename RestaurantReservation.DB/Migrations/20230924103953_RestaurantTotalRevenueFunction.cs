using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.DB.Migrations
{
    /// <inheritdoc />
    public partial class RestaurantTotalRevenueFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.parameters
                    WHERE object_id = OBJECT_ID('dbo.fn_RestaurantTotalRevenue')
                )
                    BEGIN
                        DECLARE @sql NVARCHAR(MAX);

                        SET @sql = '
                        CREATE FUNCTION fn_RestaurantTotalRevenue(@restaurant_id INT)
                        RETURNS DECIMAL(12, 4)
                        BEGIN
                            DECLARE @total_revenue DECIMAL(12, 4);
                    
                            SET @total_revenue = (
                                SELECT AVG(o.TotalAmount) AS TotalAmount
                                FROM Reservations AS r
                                         INNER JOIN Orders AS o ON r.ReservationId = o.ReservationId
                                WHERE r.RestaurantId = @restaurant_id
                            );
                    
                            RETURN @total_revenue;
                        END;
                    ';

                        EXEC sp_executesql @sql;
                    END;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS fn_RestaurantTotalRevenue;");
        }
    }
}
