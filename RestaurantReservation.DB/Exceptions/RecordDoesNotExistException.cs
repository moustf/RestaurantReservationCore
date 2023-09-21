namespace RestaurantReservation.DB.Exceptions;

public class RecordDoesNotExistException : InvalidOperationException
{
    public RecordDoesNotExistException() {  }
    
    public RecordDoesNotExistException(string message = "The user does not exist in the database!") : base (message) {  }
    
    public RecordDoesNotExistException(Exception inner, string message = "The user does not exist in the database!") : base (message, inner) {  }
}