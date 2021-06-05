using Frank.Scheduler.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frank.Scheduler.Data
{
    public class DataContext : DbContext
    {
        public DbSet<ScheduledTask> ScheduledTasks { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}
