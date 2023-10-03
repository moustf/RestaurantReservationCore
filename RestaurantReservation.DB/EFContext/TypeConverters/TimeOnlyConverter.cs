using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RestaurantReservation.DB.EFContext.TypeConverters;

public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
{
    public TimeOnlyConverter() : base(
        t => t.ToTimeSpan(),
        ts => TimeOnly.FromTimeSpan(ts)
    ) { }
}