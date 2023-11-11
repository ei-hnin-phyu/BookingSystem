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

        public async Task AddSchedule(CreateScheduleVM scheduleVM)
        {
            var package = await _dbContext.Packages.Include(u => u.Users).FirstOrDefaultAsync(x => x.Id == scheduleVM.PackageId);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == scheduleVM.UserId);
            if(package != null && user!=null && (user.NumberOfCredits - package.Fee) > 0)
            {
                if(package.Users.Count < 5)
                {
                    //book
                    user.NumberOfCredits -= package.Fee;
                    package.Users.Add(user);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    await AddToWaitList(scheduleVM);
                }

            }
        }
        private async Task AddToWaitList(CreateScheduleVM scheduleVM)
        {
            //var status = ScheduleStatus.Waitlist;
            var script = @"
                            redis.call('zadd',@userKey,@createdTime,@userId)
                         ";
            var luascript = LuaScript.Prepare(script);
            var scriptParams = new
            {
                userKey = RedisKeys.scheduleKey + scheduleVM.PackageId.ToString(),
                packageId = scheduleVM.PackageId,
                userId = scheduleVM.UserId,
                createdTime = scheduleVM.Created.ToString(),
            };
            var res = await _redisDb.ScriptEvaluateAsync(luascript, scriptParams);
        }

        public Task<List<Schedule>> GetSchedules(Country country)
        {
            throw new NotImplementedException();
        }
    }
}
