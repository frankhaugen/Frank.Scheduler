using CronQuery.Mvc.Jobs;
using Frank.Scheduler.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Frank.Scheduler.Api.Jobs
{
    public class SchedulerJob : IJob
    {
        private readonly ILogger<SchedulerJob> _logger;
        private readonly IScheduledTaskService _scheduledTaskService;


        public SchedulerJob(ILogger<SchedulerJob> logger, IScheduledTaskService scheduledTaskService)
        {
            _logger = logger;
            _scheduledTaskService = scheduledTaskService;
        }

        public async Task RunAsync()
        {
            var tasks = await _scheduledTaskService.GetPendingTasksAsync();



        }
    }
}
