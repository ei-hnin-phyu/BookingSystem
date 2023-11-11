﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Country Country { get; set; }
        public int Fee { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
