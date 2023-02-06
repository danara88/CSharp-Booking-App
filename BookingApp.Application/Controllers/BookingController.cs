using BookingApp.Application.Helpers;
using BookingApp.Application.Services.ReservationService;
using BookingApp.Domain.Models;
using ConsoleTables;
using BookingApp.Application.Enums;

namespace BookingApp.Application.Controllers
{
    public class BookingController
    {
        private readonly IReservationService _reservationService;

        public BookingController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        /// <summary>
        /// Creates a new reservation
        /// </summary>
        public void Create()
        {
            Console.Clear();
            try
            {
                var clientName = UIHelpers.GetFormValue("Name", "Client name >> ", new FormValidatorsEnum[] { FormValidatorsEnum.Required });
                var clientSurname = UIHelpers.GetFormValue("Surname", "Client surname >> ", new FormValidatorsEnum[] { FormValidatorsEnum.Required });
                var clientPhoneNumber = UIHelpers.GetFormValue("PhoneNumber", "Client phone number >> ", new FormValidatorsEnum[] { FormValidatorsEnum.Required, FormValidatorsEnum.OnlyNumbers });

                var year = DateTime.Now.Year;
                var reservationMonth = UIHelpers.GetFormValue("ReservationMonth","Reservation month >> ", new FormValidatorsEnum[] { FormValidatorsEnum.Required, FormValidatorsEnum.OnlyNumbers });
                var reservationDay = UIHelpers.GetFormValue("ReservationDay", "Reservation day >> ", new FormValidatorsEnum[] { FormValidatorsEnum.Required, FormValidatorsEnum.OnlyNumbers });
                var reservationHourMinutes = UIHelpers.GetFormValue("ReservationHour","Reservation hour (24 hour format. Ex. 16:00) >> ", new FormValidatorsEnum[] { FormValidatorsEnum.Required, FormValidatorsEnum.HoursFormat });
                var splitHourMinutes = reservationHourMinutes.Split(":");

                var reservationDate = new DateTime(year, int.Parse(reservationMonth), int.Parse(reservationDay), int.Parse(splitHourMinutes[0]), int.Parse(splitHourMinutes[1]), 0);

                var reservation = new Reservation
                {
                    ClientName = clientName,
                    ClientSurname = clientSurname,
                    ClientPhoneNumber = clientPhoneNumber,
                    ReservationDate= reservationDate,
                };

                var result = _reservationService.CreateReservation(reservation);

                if (result)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"SUCCESS: Reservation created for {clientName} {clientSurname}.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                    Console.WriteLine("Please press ENTER to continue");
                    Console.Read();
                }
                else
                {
                    UIHelpers.TriggerErrorMessage("ERROR: Something went wrong.");
                }

            }
            catch (Exception ex)
            {
                UIHelpers.TriggerErrorMessage(ex.Message);
            }

        }

        /// <summary>
        /// Lists all the reservations
        /// </summary>
        public void List()
        {
            Console.Clear();
            PrintReservationsTable();
            Console.WriteLine("Please press ENTER to continue");
            Console.Read();
        }

        /// <summary>
        /// Updates a reservation
        /// </summary>
        public void Update()
        {
            Console.Clear();
            PrintReservationsTable();
            Console.WriteLine();

            try
            {
                var reservationId = UIHelpers.GetFormValue("ReservationID", "Insert reservation ID >> ", new FormValidatorsEnum[] { FormValidatorsEnum.Required });
                Guid guidId = Guid.Parse(reservationId);
                var reservationToUpdate = _reservationService.GetReservation(guidId);
                if(reservationToUpdate is null) 
                {
                    throw new Exception($"ERROR: The reservation with ID {reservationId} does not exist.");
                }
                Console.WriteLine();

                var clientName = UIHelpers.GetFormValue("Name", $"Client name [{reservationToUpdate.ClientName}] >> ");
                if (string.IsNullOrEmpty(clientName)) clientName = reservationToUpdate.ClientName;

                var clientSurname = UIHelpers.GetFormValue("Surname", $"Client surname [{reservationToUpdate.ClientSurname}] >> ");
                if (string.IsNullOrEmpty(clientSurname)) clientSurname = reservationToUpdate.ClientSurname;

                var clientPhoneNumber = UIHelpers.GetFormValue("PhoneNumber", $"Client phone number [{reservationToUpdate.ClientPhoneNumber}] >> ", new FormValidatorsEnum[] { FormValidatorsEnum.OnlyNumbers });
                if (string.IsNullOrEmpty(clientPhoneNumber)) clientPhoneNumber = reservationToUpdate.ClientPhoneNumber;

                var reservationYear = UIHelpers.GetFormValue("ReservationYear", $"Reservation year [{reservationToUpdate.ReservationDate.ToString("yyyy")}] >> ", new FormValidatorsEnum[] { FormValidatorsEnum.OnlyNumbers });
                if (string.IsNullOrEmpty(reservationYear)) reservationYear = reservationToUpdate.ReservationDate.ToString("yyyy");

                var reservationMonth = UIHelpers.GetFormValue("ReservationMonth", $"Reservation month [{reservationToUpdate.ReservationDate.ToString("MMM")}] >> ", new FormValidatorsEnum[] { FormValidatorsEnum.OnlyNumbers });
                if (string.IsNullOrEmpty(reservationMonth)) reservationMonth = reservationToUpdate.ReservationDate.ToString("MM");

                var reservationDay = UIHelpers.GetFormValue("ReservationDay", $"Reservation day [{reservationToUpdate.ReservationDate.ToString("dd")}] >> ", new FormValidatorsEnum[] { FormValidatorsEnum.OnlyNumbers });
                if (string.IsNullOrEmpty(reservationDay)) reservationDay = reservationToUpdate.ReservationDate.ToString("dd");

                var reservationHourMinutes = UIHelpers.GetFormValue("ReservationHour", $"Reservation hour (24 hour format. Ex. 16:00)  [{reservationToUpdate.ReservationDate.ToString("hh:mm:ss")}] >> ", new FormValidatorsEnum[] { FormValidatorsEnum.HoursFormat });
                if (string.IsNullOrEmpty(reservationHourMinutes)) reservationHourMinutes = reservationToUpdate.ReservationDate.ToString("hh:mm:ss");

                var splitHourMinutes = reservationHourMinutes.Split(":");

                var reservationDate = new DateTime(int.Parse(reservationYear), int.Parse(reservationMonth), int.Parse(reservationDay), int.Parse(splitHourMinutes[0]), int.Parse(splitHourMinutes[1]), 0);

                var reservation = new Reservation
                {
                    Id = reservationToUpdate.Id,
                    ClientName = clientName,
                    ClientSurname = clientSurname,
                    ClientPhoneNumber = clientPhoneNumber,
                    ReservationDate = reservationDate,
                };

                var result = _reservationService.UpdateReservation(reservation);

                if (result)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"SUCCESS: Reservation updated for {clientName} {clientSurname} with ID {reservationToUpdate.Id}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                    Console.WriteLine("Please press ENTER to continue");
                    Console.Read();
                }
                else
                {
                    UIHelpers.TriggerErrorMessage("ERROR: Something went wrong.");
                }

            }
            catch (Exception ex)
            {
                UIHelpers.TriggerErrorMessage(ex.Message);
            }

            
        }

        /// <summary>
        /// Deletes a reservation
        /// </summary>
        public void Delete()
        {
            Console.Clear();
            PrintReservationsTable();
            Console.WriteLine();
            try
            {
                var reservationId = UIHelpers.GetFormValue("ReservationID", "Insert reservation ID >> ", new FormValidatorsEnum[] { FormValidatorsEnum.Required });
                Guid guidId = Guid.Parse(reservationId);
                var result = _reservationService.DeleteReservation(guidId);
                if (result)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"SUCCESS: The reservation with ID {reservationId} was deleted.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                    Console.WriteLine("Please press ENTER to continue");
                    Console.Read();
                }
                else
                {
                    UIHelpers.TriggerErrorMessage("ERROR: Something went wrong.");
                }
            }
            catch (Exception ex)
            {
                UIHelpers.TriggerErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Prints the reservations in a table view
        /// </summary>
        private void PrintReservationsTable()
        {
            var table = new ConsoleTable("ID", "Client name", "Client surname", "Client phone number", "Reservation date");
            var reservations = _reservationService.GetReservations();

            foreach (var reservation in reservations)
            {
                table.AddRow(reservation.Id, reservation.ClientName, reservation.ClientSurname, reservation.ClientPhoneNumber, reservation.ReservationDate);
            }

            table.Write();
            Console.WriteLine();
        }
    }
}
