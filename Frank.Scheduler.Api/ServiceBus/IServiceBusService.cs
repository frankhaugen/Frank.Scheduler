using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Frank.Scheduler.Api.ServiceBus
{
    public interface IServiceBusService
    {
        Task<Message> SendMessage<T>(Guid messageId, string messageLabel, T body);
        Task<Message> SendMessage(Guid messageId, string messageLabel, string messageBody);
    }
}