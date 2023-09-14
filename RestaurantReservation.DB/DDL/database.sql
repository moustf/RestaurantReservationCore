IF NOT EXISTS(
    SELECT 1
    FROM sys.sysdatabases
    WHERE name = 'RestaurantReservationCore'
)
BEGIN
    CREATE DATABASE RestaurantReservationCore;
END;
