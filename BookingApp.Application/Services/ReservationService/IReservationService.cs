using BookingApp.Domain.Models;

namespace BookingApp.Application.Services.ReservationService
{
    public interface IReservationService
    {
        public bool CreateReservation(Reservation reservation);

        public List<Reservation> GetReservations();

        public Reservation GetReservation(Guid reservationId);

        public bool UpdateReservation(Reservation reservation);

        public bool DeleteReservation(Guid reservationId);
    }
}
