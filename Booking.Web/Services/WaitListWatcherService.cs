using Booking.Model;
using Booking.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using StackExchange.Redis;

namespace Booking.Web.Services
{
    public class WaitListWatcherService
    {
        private readonly IDatabase _redisDb;
        private readonly BookingDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public WaitListWatcherService(IDatabase database, BookingDbContext contextFactory,IConfiguration config) {
            _redisDb = database;
            _dbContext = contextFactory;
            _configuration = config;
        }
        public async Task CheckWaitListAsync()
        {
            var availablePackage = new List<Package>();
            availablePackage = await _dbContext.Packages.Include(p => p.Users).Where(p => p.Users.Count < int.Parse(_configuration["MaxBookingCount"])).ToListAsync();


            foreach (var package in availablePackage)
            {
                var script = @"
                        local first_element = redis.call(""ZRANGE"", @userKey, 0, 0)

                        if #first_element > 0 then
                            redis.call(""ZREM"", @userKey, first_element[1])
                        end

                        return first_element[1]                            
                         ";
                var luascript = LuaScript.Prepare(script);
                var scriptParams = new
                {
                    userKey = RedisKeys.scheduleKey + package.Id.ToString(),
                };
                var res = await _redisDb.ScriptEvaluateAsync(luascript, scriptParams);
                var userId = (int?)res;
                if (userId != null)
                {
                    var schedule = await _dbContext.Schedules.FirstOrDefaultAsync(x => x.PackageId == package.Id && x.UserId == userId);
                    if (schedule != null)
                    {
                        schedule.Status = Model.ScheduleStatus.Book;
                        _dbContext.Schedules.Update(schedule);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }


        }
    }
}
