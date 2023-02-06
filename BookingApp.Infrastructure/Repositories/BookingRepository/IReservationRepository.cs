using BookingApp.Domain.Models;

namespace BookingApp.Infrastructure.Repositories.BookingRepository
{
    public interface IReservationRepository
    {
        public string CreateReservation(Reservation input);

        public List<Reservation> GetReservations();

        public Reservation UpdateReservation(Reservation input);

        public Reservation GetReservation(Guid reservationId);

        public Reservation DeleteReservation(Guid reservationId);
    }
}
