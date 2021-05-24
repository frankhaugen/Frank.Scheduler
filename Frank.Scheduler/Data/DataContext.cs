using Microsoft.EntityFrameworkCore;

namespace Frank.Scheduler.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}
