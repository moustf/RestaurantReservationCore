namespace RestaurantReservation.DB.Exceptions;

public class RecordExistsException : InvalidOperationException
{
    public RecordExistsException() {  }
    
    public RecordExistsException(string message = "The user already exists in the database!") : base (message) {  }
    
    public RecordExistsException(Exception inner, string message = "The user already exists in the database!") : base (message, inner) {  }
}