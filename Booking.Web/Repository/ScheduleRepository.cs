using Booking.Model;
using Booking.Web.Models;
using Booking.Web.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Booking.Web.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly BookingDbContext _dbContext;
        private readonly IDatabase _redisDb;
        public ScheduleRepository(BookingDbContext dbContext, IDatabase redisDb)
        {
            _dbContext = dbContext;
            this._redisDb = redisDb;
        }

        public async Task CreateBooking(CreateScheduleVM scheduleVM)
        {
            var package = await _dbContext.Packages.Include(u => u.Users).FirstOrDefaultAsync(x => x.Id == scheduleVM.PackageId);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == scheduleVM.UserId);
            if(package != null && user!=null && (user.NumberOfCredits - package.Fee) >= 0)
            {
                var schedule = new Schedule
                {
                    PackageId = package.Id,
                    ScheduleTime = DateTime.UtcNow,
                    UserId = user.Id,
                };
                if (package.Users.Count < 5)
                {
                    //book
                    user.NumberOfCredits -= package.Fee;
                    package.Users.Add(user);
                    schedule.Status = Model.ScheduleStatus.Book;
                }
                else
                {
                    schedule.Status = Model.ScheduleStatus.Waitlist;
                    await AddToWaitList(scheduleVM);
                }
                _dbContext.Users.Update(user);
                await _dbContext.Schedules.AddAsync(schedule);
                await _dbContext.SaveChangesAsync();

            }
        }
        private async Task AddToWaitList(CreateScheduleVM scheduleVM)
        {
            var unixTimestamp = (int)scheduleVM.Created.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var script = @"
                            redis.call('zadd',@userKey,@createdTime,@userId)
                         ";
            var luascript = LuaScript.Prepare(script);
            var scriptParams = new
            {
                userKey = RedisKeys.scheduleKey + scheduleVM.PackageId.ToString(),
                packageId = scheduleVM.PackageId,
                userId = scheduleVM.UserId,
                createdTime = unixTimestamp,
            };
            var res = await _redisDb.ScriptEvaluateAsync(luascript, scriptParams);
        }
        public async Task CancelBooking(int id, int userId)
        {
            var schedule = await this._dbContext.Schedules.Include(x=> x.Package).Include(x => x.User).Where(x => x.Id == id && x.User.Id == userId).FirstOrDefaultAsync();
            if (schedule != null)
            {
                var user = await _dbContext.Users.Include(x => x.Packages).FirstOrDefaultAsync(x => x.Id == schedule.UserId);
                var package = schedule.Package;
                if(schedule.ScheduleTime.Subtract(DateTime.UtcNow).TotalHours >= 4)
                {
                    user.NumberOfCredits += package.Fee;
                }
                user.Packages.Remove(package);
                _dbContext.Schedules.Remove(schedule);
                _dbContext.Users.Update(user);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Schedule>> GetBooking(int userId)
        {
            return await this._dbContext.Schedules.Include(x => x.Package).Where(x => x.User.Id == userId && x.Status == Model.ScheduleStatus.Book).ToListAsync();
        }

        public async Task<List<Schedule>> GetWaitList(int userId)
        {
            return await this._dbContext.Schedules.Include(x => x.Package).Where(x => x.User.Id == userId && x.Status == Model.ScheduleStatus.Waitlist).ToListAsync();
        }
    }
}
