namespace BookingApp.Domain.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }

        public string? ClientName { get; set; }

        public string? ClientSurname { get; set; }

        public string? ClientPhoneNumber { get; set; }

        public DateTime ReservationDate { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
