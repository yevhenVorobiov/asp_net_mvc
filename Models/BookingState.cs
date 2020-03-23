using System.Collections.Generic;

namespace Hotel.Models
{
    public enum BookingState
    {
        CLOSED = 1,
        PENDING = 2,
        DECLINED = 3,
        IN_PROGRESS = 4,
        BOOKED = 5
    }

    public class StateUtils {
        public static IEnumerable<BookingState> getPossibleSates(BookingState currentState) {
            switch (currentState) {
                case BookingState.PENDING :
                    return new List<BookingState>() {
                        BookingState.PENDING, BookingState.BOOKED, BookingState.DECLINED
                    };
                case BookingState.BOOKED:
                    return new List<BookingState>() {
                        BookingState.BOOKED, BookingState.IN_PROGRESS,  BookingState.DECLINED
                    };
                case BookingState.IN_PROGRESS:
                    return new List<BookingState>() {
                        BookingState.IN_PROGRESS, BookingState.CLOSED
                    };
                default:
                    return new List<BookingState>();
            }
        }
    }
}
