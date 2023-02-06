using BookingApp.Domain.Models;
using BookingApp.Infrastructure.Repositories.BookingRepository;

namespace BookingApp.Application.Services.ReservationService
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRespository;
        public ReservationService(IReservationRepository reservationRespository)
        {
            _reservationRespository = reservationRespository;
        }

        /// <summary>
        /// Creates a reservation
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public bool CreateReservation(Reservation reservation)
        {
            try
            {
                _reservationRespository.CreateReservation(reservation);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Gets a reservation by id
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        public Reservation GetReservation(Guid reservationId)
        {
            return _reservationRespository.GetReservation(reservationId);
        }

        /// <summary>
        /// Gets all the reservations
        /// </summary>
        /// <returns></returns>
        public List<Reservation> GetReservations()
        {
            return _reservationRespository.GetReservations();
        }


        /// <summary>
        /// Updates a reservation
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public bool UpdateReservation(Reservation reservation)
        {
            try
            {
                _reservationRespository.UpdateReservation(reservation);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a reservation by id
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        public bool DeleteReservation(Guid reservationId) 
        {
            try
            {
                _reservationRespository.DeleteReservation(reservationId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
