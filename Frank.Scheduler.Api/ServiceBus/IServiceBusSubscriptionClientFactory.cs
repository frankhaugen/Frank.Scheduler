using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Frank.Scheduler.Api.ServiceBus
{
    public interface IServiceBusSubscriptionClientFactory
    {
        Task<ISubscriptionClient> CreateSubscriptionClientAsync(string serviceBusEndpoint, string topicName, string subscriptionName, string filter, short lockDuration);
    }
}