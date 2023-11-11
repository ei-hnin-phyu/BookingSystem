namespace Booking.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int NumberOfCredits { get; set; }
        public string? PhoneNumber { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Package> Packages { get; set; }
    }
}