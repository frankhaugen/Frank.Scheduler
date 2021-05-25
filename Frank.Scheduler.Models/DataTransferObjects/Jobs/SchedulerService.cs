using FluentScheduler;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Frank.Scheduler.Api.Jobs
{
    public class SchedulerService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            JobManager.AddJob(async () => await CheckSchedules(), schedule => schedule.ToRunEvery(1).Minutes());

            return Task.CompletedTask;
        }

        private async Task CheckSchedules()
        {

        }
    }
}
