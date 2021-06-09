using System.Threading.Tasks;
using Frank.Scheduler.Models.Messages;

namespace Frank.Scheduler.Client.ServiceBus.Producers
{
    public interface ISchedulerResponseProducer
    {
        Task ProduceMessage(SchedulerCallbackMessage message, string label);
    }
}