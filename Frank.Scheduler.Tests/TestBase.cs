using Frank.Scheduler.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using Xunit.Abstractions;

namespace Frank.Scheduler.Tests
{
    public class TestBase
    {
        private readonly ITestOutputHelper _outputHelper;

        public TestBase(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        protected void Output(string value) => _outputHelper.WriteLine(value);
        protected void Output(object value) => _outputHelper.WriteLine(JsonSerializer.Serialize(value, new JsonSerializerOptions() { WriteIndented = true }));

        protected ITestOutputHelper TestOutputHelper => _outputHelper;

        protected DataContext CreateTestDatabase(bool ensureDeletion = false)
        {
            //var context = DataContext(new DbContextOptionsBuilder<DataContext>().EnableSensitiveDataLogging().UseSqlServer("Server=dev-westeurope-sqlserver.database.windows.net;Database=integrationengine;User Id=integrationengineuser;Password=rp3EhuV8xDe0ui952MJh;").Options);
            var context = new DataContext(new DbContextOptionsBuilder<DataContext>().EnableSensitiveDataLogging().EnableDetailedErrors().UseSqlServer("Server=localhost\\SQLEXPRESS;Database=FrankScheduler;Trusted_Connection=True;").Options);
            //var context = new DataContext(new DbContextOptionsBuilder<DataContext>().EnableSensitiveDataLogging().UseInMemoryDatabase(RandomString(10)).Options);
            //var context = InMemoryContextBuilder.Build<DataContext>(new DbContextOptionsBuilder().EnableDetailedErrors().EnableSensitiveDataLogging().UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll));

            //_outputHelper.WriteLine(context.Database.GenerateCreateScript());

            if (ensureDeletion)
            {
                context.Database.EnsureDeleted();
            }
            context.Database.EnsureCreated();

            return context;
        }

        private static readonly Random Random = new Random();

        protected static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
