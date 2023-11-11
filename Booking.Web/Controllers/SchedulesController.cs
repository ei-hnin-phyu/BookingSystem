using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking.Model;
using Booking.Web.Models;
using Booking.Web.Repository.Interface;

namespace Booking.Web.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly BookingDbContext _context;
        private readonly IScheduleRepository _scheduleRepository;

        public SchedulesController(BookingDbContext context,IScheduleRepository scheduleRepository)
        {
            _context = context;
            _scheduleRepository = scheduleRepository;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var bookingDbContext = _context.Schedules.Include(s => s.Package).Include(s => s.User);
            return View(await bookingDbContext.ToListAsync());
        }
        public async Task<IActionResult> AllBookings()
        {
            var userid = GetUserId();
            if (userid == null)
            {
                return NotFound();
            }
            var usrId = (int)userid;
            return View(await _scheduleRepository.GetBooking(usrId));
        }
        public async Task<IActionResult> Book(int? id)
        {
            var userid = GetUserId();
            if(userid == null)
            {
                return NotFound();
            }
            var usrId = (int)userid;
            await _scheduleRepository.CreateBooking(new CreateScheduleVM
            {
                Created = DateTime.UtcNow,
                PackageId = (int)id,
                UserId = usrId,
            });
            return RedirectToAction("Index", "Packages", new { });
        }
        private int? GetUserId()
        {
            var userid = String.Empty;
            var cookie = Request.Cookies.TryGetValue(Constants.UserIdCookie, out userid);
            if(cookie)
                return int.Parse(userid);
            return null;
        }
        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Package)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }
        public async Task<IActionResult> CancelBooking(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int scheId = (int)id;
            var userId = GetUserId();
            if (userId == null)
            {
                return NotFound();
            }
            await _scheduleRepository.CancelBooking(scheId, (int)userId);

            return RedirectToAction(nameof(AllBookings));
        }        
        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Schedules == null)
            {
                return Problem("Entity set 'BookingDbContext.Schedules'  is null.");
            }
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
          return (_context.Schedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
