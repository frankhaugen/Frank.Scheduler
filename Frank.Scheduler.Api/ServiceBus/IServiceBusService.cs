using System.Threading.Tasks;

namespace Frank.Scheduler.Api.ServiceBus
{
    public interface IServiceBusService
    {
        Task SendMessage(string message);
    }
}