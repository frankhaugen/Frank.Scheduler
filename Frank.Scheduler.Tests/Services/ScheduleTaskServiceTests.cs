using Frank.Scheduler.Data;
using Frank.Scheduler.Services;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Frank.Scheduler.Tests.Services
{
    public class ScheduleTaskServiceTests : TestBase
    {
        private readonly DataContext _dataContext;

        public ScheduleTaskServiceTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            _dataContext = CreateTestDatabase();
        }

        private ScheduledTaskService CreateService() => new(_dataContext, TestOutputHelper.BuildLoggerFor<ScheduledTaskService>());

        [Fact]
        public async Task GetTasksAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var stopwatch = new Stopwatch();

            //stopwatch.Start();

            //var cron = CronExpression.Parse("@every_minute");

            //var fakes = new Faker<ScheduledTask>().Generate(1000 * 1000);
            //fakes.ForEach(x =>
            //{
            //    x.LastRun = DateTime.UtcNow.AddSeconds(RandomNumberGenerator.GetInt32(-60, 60));
            //    x.Cron = cron.ToString();
            //    x.Label = "ALabel";
            //});
            //_dataContext.ScheduledTasks.AddRange(fakes);
            //await _dataContext.SaveChangesAsync();
            //stopwatch.Stop();
            //Output($"Setup time: {stopwatch.Elapsed}");
            //stopwatch.Reset();

            var service = CreateService();

            // Act
            stopwatch.Start();
            var result = await service.GetPendingTasksAsync();
            stopwatch.Stop();

            // Assert
            Output($"Test time: {stopwatch.Elapsed}");
            Output(result.Count());
        }

    }
}
