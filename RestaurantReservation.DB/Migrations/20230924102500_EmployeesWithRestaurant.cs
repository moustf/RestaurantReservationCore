using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.DB.Migrations
{
    /// <inheritdoc />
    public partial class EmployeesWithRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF NOT EXISTS(
                    SELECT 1
                    FROM INFORMATION_SCHEMA.VIEWS
                    WHERE TABLE_NAME = 'EmployeesWithRestaurant'
                )
                BEGIN
                    DECLARE @sql NVARCHAR(MAX);
                    SET @sql = '
                        CREATE VIEW EmployeesWithRestaurant
                        AS
                        SELECT e.EmployeeId,
                               e.FirstName + '' '' + e.LastName AS FullName,
                               e.Position,
                               r.RestaurantId,
                               r.Name,
                               r.Address,
                               r.PhoneNumber,
                               r.OpeningHours
                        FROM
                            Employees AS e
                                INNER JOIN Restaurants AS r ON e.RestaurantId = r.RestaurantId; 
                    ';
                    
                    EXEC sp_executesql @sql;
                END;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS EmployeesWithRestaurant");
        }
    }
}
