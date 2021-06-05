using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Frank.Scheduler.Client.ServiceBus.Consumers
{
    public class ServiceBusSubscriptionClientFactory : IServiceBusSubscriptionClientFactory
    {
        public async Task<ISubscriptionClient> CreateSubscriptionClientAsync(string serviceBusEndpoint, string topicName, string subscriptionName, string filter, short lockDuration)
        {
            await CreateSubscriptionIfNotExistsAsync(serviceBusEndpoint, topicName, subscriptionName, lockDuration);
            var subscriptionClient = new SubscriptionClient(serviceBusEndpoint, topicName, subscriptionName);
            await ValidateSubscriptionRulesAsync(subscriptionClient, filter);
            return subscriptionClient;
        }

        private async Task CreateSubscriptionIfNotExistsAsync(string serviceBusEndpoint, string topicName, string subscriptionName, short lockDuration)
        {
            var client = new ManagementClient(serviceBusEndpoint);
            var subscriptionExists = await client.SubscriptionExistsAsync(topicName, subscriptionName);

            if (subscriptionExists == false)
            {
                await client.CreateSubscriptionAsync(
                    new SubscriptionDescription(topicName, subscriptionName)
                    {
                        LockDuration = TimeSpan.FromMinutes(lockDuration)
                    });
            }

            await client.CloseAsync();
        }

        private async Task ValidateSubscriptionRulesAsync(ISubscriptionClient subscriptionClient, string filterName)
        {
            var filterExists = false;
            var hasDefaultFilter = false;
            var filterCount = 0;

            foreach (var rule in await subscriptionClient.GetRulesAsync())
            {
                if (rule.Name == filterName && rule.Filter is CorrelationFilter filter &&
                    filter.Label == filterName)
                {
                    filterExists = true;
                }

                if (rule.Name == "$Default")
                {
                    hasDefaultFilter = true;
                }

                filterCount++;
            }

            if (hasDefaultFilter && filterName == "all")
            {
                return;
            }

            if (filterCount != 1 && filterExists || filterCount != 1 && hasDefaultFilter)
            {
                throw new Exception("Invalid combination of subscription name and filter");
            }

            if (hasDefaultFilter)
            {
                await subscriptionClient.RemoveRuleAsync("$Default");
            }

            if (filterExists == false)
            {
                await subscriptionClient.AddRuleAsync(new RuleDescription(filterName, new CorrelationFilter
                {
                    ContentType = "application/json",
                    Label = filterName
                }));
            }
        }
    }
}