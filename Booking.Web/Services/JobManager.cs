using Booking.Web.Models;
using Hangfire;

namespace Booking.Web.Services
{
    public class JobManager : IHostedService
    {
        private readonly IConfiguration _configuration;
        public JobManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var timeout = (int.Parse(this._configuration["WaitlistTimeoutMin"])) + 1;
            var cron_exp = this._configuration["Cron_WaitList"];
            RecurringJob.AddOrUpdate<WaitListWatcherService>(Constants.WaitListJob, (x) => x.CheckWaitListAsync(), cron_exp);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            RecurringJob.RemoveIfExists(Constants.WaitListJob);
            return Task.CompletedTask;
        }
    }
}
