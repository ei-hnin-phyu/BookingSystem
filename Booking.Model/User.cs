﻿namespace Booking.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int NumberOfCredits { get; set; }
        public Country Country { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Package> Packages { get; set; }
    }
}