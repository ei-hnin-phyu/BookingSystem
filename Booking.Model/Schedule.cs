using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime ScheduleTime { get; set; }
        public ScheduleStatus Status { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int PackageId { get; set; }
        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; }
    }
    public enum ScheduleStatus
    {
        Book,
        Waitlist,
        CancelBooking,
        CheckIn
    }
}
