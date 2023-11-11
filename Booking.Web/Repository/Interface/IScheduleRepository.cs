using Booking.Model;
using Booking.Web.Models;

namespace Booking.Web.Repository.Interface
{
    public interface IScheduleRepository
    {
        Task<List<Schedule>> GetBooking(int userId);
        Task<List<Schedule>> GetWaitList(int userId);
        Task CreateBooking(CreateScheduleVM scheduleVM);
        Task CancelBooking(int id,int userId);
    }
}
