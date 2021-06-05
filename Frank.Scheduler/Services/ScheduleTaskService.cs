using Cronos;
using Frank.Scheduler.Data;
using Frank.Scheduler.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frank.Scheduler.Services
{
    class ScheduleTaskService
    {
        private readonly DataContext _dataContext;

        public ScheduleTaskService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<ScheduledTask>> GetTasksAsync()
        {
            var tasks = _dataContext
                .ScheduledTasks
                .AsNoTracking()
                .Where(x => x.State == State.Running)
                .Where(x => CronExpression.Parse(x.Cron, CronFormat.IncludeSeconds).GetNextOccurrence(x.LastRun.Value, true) < DateTime.UtcNow
                );

            return await tasks.ToListAsync();
        }
    }
}
