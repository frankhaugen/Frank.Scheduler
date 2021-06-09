using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Frank.Scheduler.Data.Entities;

namespace Frank.Scheduler.Services
{
    public interface IScheduledTaskService
    {
        Task<ScheduledTask?> DeleteTaskAsync(Guid id);
        Task<ScheduledTask?> AddTaskAsync(ScheduledTask scheduledTask);
        Task<ScheduledTask?> UpsertTaskAsync(ScheduledTask scheduledTask);
        Task<ScheduledTask?> UpdateTaskAsync(ScheduledTask scheduledTask);
        Task<ScheduledTask?> Get(Guid id);
        Task<List<ScheduledTask>> Get(int skip = 0, int take = 1000);
        Task<IEnumerable<ScheduledTask>> GetPendingTasksAsync();
    }
}