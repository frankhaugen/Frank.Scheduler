using Frank.Scheduler.Models.Messages;
using System;
using System.Threading.Tasks;

namespace Frank.Scheduler.Client.ServiceBus.Interfaces
{
    public interface ISchedulerHandler
    {
        Task<SchedulerResult> TriggerScheduledTask(Guid id);
    }
}
