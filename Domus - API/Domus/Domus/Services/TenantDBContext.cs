using Domus.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Domus.Services
{
    public interface ITenantDbContextFactory
    {
        ApplicationDbContext Create(string connectionString);
    }

    public class TenantDBContext : ITenantDbContextFactory
    {
        public ApplicationDbContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 41)))
                          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                          .EnableThreadSafetyChecks(false);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
