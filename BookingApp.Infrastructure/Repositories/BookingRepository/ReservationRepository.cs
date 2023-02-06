using BookingApp.Domain.Models;
using System.Text;

namespace BookingApp.Infrastructure.Repositories.BookingRepository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly string _reservationDBPath = @"C:\Users\danie\Documents\Programacion\ProyectosPersonales\BookingApp\BookingApp.Infrastructure\Database\Reservations.csv";

        public ReservationRepository()
        {
        }

        /// <summary>
        /// Creates a reservation
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public string CreateReservation(Reservation reservation)
        {
            try
            {
                reservation.Id = Guid.NewGuid();
                using (var streamWriter = new StreamWriter(_reservationDBPath, append: true, encoding: Encoding.UTF8))
                {
                    streamWriter.WriteLine($"{reservation.Id},{reservation.ClientName},{reservation.ClientSurname},{reservation.ClientPhoneNumber},{reservation.ReservationDate},{reservation.CreatedOn}");
                }
                return reservation.Id.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong.");
            }
        }

        /// <summary>
        /// Gets all the reservations
        /// </summary>
        /// <returns></returns>
        public List<Reservation> GetReservations()
        {
            var reservations = new List<Reservation>();
            using (var streamReader = new StreamReader(_reservationDBPath))
            {
                string line;
                int i = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (i >= 1)
                    {
                        var reservation = ReadFromCSVFile(line);
                        reservations.Add(reservation);
                    }
                    i++;
                }
            }
            reservations = reservations.OrderBy(reservation => reservation.ReservationDate).ToList();
            return reservations;
        }

        /// <summary>
        /// Gets a reservation by Id
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        public Reservation GetReservation(Guid reservationId)
        {
            var reservations = GetReservations();
            var reservation = reservations.FirstOrDefault(r => r.Id == reservationId);
            return reservation;
        }

        /// <summary>
        /// Updates a reservation
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public Reservation UpdateReservation(Reservation input)
        {
            var reservations = GetReservations();
            var reservationToUpdate = GetReservation(input.Id);
            var reservationIndex = reservations.FindIndex(r => r.Id == input.Id);
            if(reservationToUpdate is not null) 
            {
                reservationToUpdate.ClientName = input.ClientName;
                reservationToUpdate.ClientSurname = input.ClientSurname;
                reservationToUpdate.ClientPhoneNumber = input.ClientPhoneNumber;
                reservationToUpdate.ReservationDate = input.ReservationDate;
            }
            reservations.RemoveAt(reservationIndex);
            reservations.Add(reservationToUpdate!);
            OverwriteDatabase(reservations);
            return reservationToUpdate!;
        }
        
        /// <summary>
        /// Deletes a reservation
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Reservation DeleteReservation(Guid reservationId)
        {
            var reservations = GetReservations();
            try
            {
                var reservation = reservations.FirstOrDefault(r => r.Id == reservationId);
                if(reservation is not null)
                {
                    var reservationIndex = reservations.FindIndex(r => r.Id == reservationId);
                    reservations.RemoveAt(reservationIndex);
                    OverwriteDatabase(reservations);
                }
                else
                {
                    throw new Exception("ERROR: The reservation does not exist.");
                }
                return reservation;
            }
            catch (Exception)
            {
                throw new Exception("ERROR: Something went wrong.");
            }
        }

        /// <summary>
        /// Reads a line form CSV to format into Reservation
        /// </summary>
        /// <param name="csvLine"></param>
        /// <returns></returns>
        private Reservation ReadFromCSVFile(string csvLine)
        {
            string[] values = csvLine.Split(',');
            var reservation = new Reservation();
            reservation.Id = Guid.Parse(values[0]);
            reservation.ClientName = values[1];
            reservation.ClientSurname= values[2];
            reservation.ClientPhoneNumber = values[3];
            reservation.ReservationDate = DateTime.Parse(values[4]);
            return reservation;
        }

        /// <summary>
        /// This method will overwrite the entire database
        /// </summary>
        /// <param name="reservations"></param>
        /// <exception cref="Exception"></exception>
        private void OverwriteDatabase(List<Reservation> reservations)
        {
            try
            {
                using (var streamWriter = new StreamWriter(_reservationDBPath, append: false, encoding: Encoding.UTF8))
                {
                    streamWriter.WriteLine("Id,Client name,Client surname,Client phone number,Reservation date,Created on");
                    foreach (var reservation in reservations)
                    {
                        streamWriter.WriteLine($"{reservation.Id},{reservation.ClientName},{reservation.ClientSurname},{reservation.ClientPhoneNumber},{reservation.ReservationDate},{reservation.CreatedOn}");
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("ERROR: Something went wrong.");
            }
        }
    }
}
