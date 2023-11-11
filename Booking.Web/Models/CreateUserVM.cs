using Booking.Model;

namespace Booking.Web.Models
{
    public class CreateUserVM
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int NumberOfCredits { get; set; }

    }
    public class EditUserVM
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfCredits { get; set; }

    }
}
