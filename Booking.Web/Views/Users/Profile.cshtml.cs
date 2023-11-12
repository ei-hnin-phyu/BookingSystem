using Booking.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.Web.Views.Users
{
    public class ProfileModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int NumberOfCredits { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
        public List<Package> Packages { get; set; } = new List<Package>();
    }
}
