using Booking.Model;
using Booking.Web.Models;

namespace Booking.Web.Repository.Interface
{
    public interface IScheduleRepository
    {
        Task<List<Schedule>> GetSchedules(Country country);
        Task AddSchedule(CreateScheduleVM scheduleVM);
    }
}
