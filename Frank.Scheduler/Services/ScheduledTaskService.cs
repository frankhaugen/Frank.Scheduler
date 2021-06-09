using Cronos;
using Frank.Scheduler.Constants;
using Frank.Scheduler.Data;
using Frank.Scheduler.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Frank.Scheduler.Services
{
    public class ScheduledTaskService : IScheduledTaskService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ScheduledTaskService> _logger;

        public ScheduledTaskService(DataContext dataContext, ILogger<ScheduledTaskService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task<ScheduledTask?> DeleteTaskAsync(Guid id)
        {
            var task = await _dataContext.ScheduledTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null) return task;

            var entry = _dataContext.Remove(task);
            var rows = await _dataContext.SaveChangesAsync();
            return rows == 1 ? entry.Entity : default;
        }

        public async Task<ScheduledTask?> AddTaskAsync(ScheduledTask scheduledTask)
        {
            var entry = await _dataContext.ScheduledTasks.AddAsync(scheduledTask);
            var rows = await _dataContext.SaveChangesAsync();
            return rows == 1 ? entry.Entity : default;
        }

        public async Task<ScheduledTask?> UpsertTaskAsync(ScheduledTask scheduledTask)
        {
            var task = await _dataContext.ScheduledTasks.FirstOrDefaultAsync(x => x.Id == scheduledTask.Id);
            if (task == null) return await AddTaskAsync(scheduledTask);

            task.LastRun = scheduledTask.LastRun;
            task.Cron = scheduledTask.Cron;
            task.Label = scheduledTask.Label;
            task.State = scheduledTask.State;

            var entry = _dataContext.ScheduledTasks.Update(scheduledTask);
            var rows = await _dataContext.SaveChangesAsync();
            return rows == 1 ? entry.Entity : default;
        }

        public async Task<ScheduledTask?> UpdateTaskAsync(ScheduledTask scheduledTask)
        {
            var task = await Get(scheduledTask.Id);
            if (task == null) return task;

            task.LastRun = scheduledTask.LastRun;
            task.Cron = scheduledTask.Cron;
            task.Label = scheduledTask.Label;
            task.State = scheduledTask.State;

            var entry = _dataContext.ScheduledTasks.Update(scheduledTask);
            var rows = await _dataContext.SaveChangesAsync();
            return rows == 1 ? entry.Entity : default;
        }

        public async Task<ScheduledTask?> Get(Guid id) => await _dataContext.ScheduledTasks.SingleOrDefaultAsync(x => x.Id == id);
        public async Task<List<ScheduledTask>> Get(int skip = 0, int take = 1000) => await _dataContext.ScheduledTasks.AsNoTracking().OrderByDescending(x => x.LastRun).Skip(skip).Take(take).ToListAsync();

        public async Task<IEnumerable<ScheduledTask>> GetPendingTasksAsync()
        {
            var count = await _dataContext.ScheduledTasks.CountAsync();
            _logger.LogInformation($"{count} tasks discovered");
            var stopwatch = Stopwatch.StartNew();
            if (count > Numbers.OneHundredThousand)
            {
                // TODO: Implement batching on 100 000 and run parallel evaluation
            }

            var query = _dataContext
                .ScheduledTasks
                .AsNoTracking()
                .Where(x => x.State == State.Running)
                .AsEnumerable()
                .Where(x => x.LastRun.HasValue && Check(x.Cron, DateTime.SpecifyKind(x.LastRun.Value, DateTimeKind.Utc)) < DateTime.UtcNow);

            stopwatch.Stop();
            _logger.LogInformation($"Processing {count} tasks, discovering {query.Count()} to be run");

            return query;
        }

        private DateTime Check(string cron, DateTime dateTime)
        {
            DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            var result = CronExpression.Parse(cron, CronFormat.IncludeSeconds).GetNextOccurrence(dateTime, true).GetValueOrDefault();
            DateTime.SpecifyKind(result, DateTimeKind.Utc);
            return result;
        }
    }
}
