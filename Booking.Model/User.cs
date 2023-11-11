namespace Booking.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int NumberOfCredits { get; set; }
        public string? PhoneNumber { get; set; }
        public virtual List<Schedule> Schedules { get; set; }
        public virtual List<Package> Packages { get; set; }
    }
}