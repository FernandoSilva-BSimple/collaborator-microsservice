using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class AbsanteeContextFactory : IDesignTimeDbContextFactory<AbsanteeContext>
    {
        public AbsanteeContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<AbsanteeContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AbsanteeContext(optionsBuilder.Options);
        }
    }
}
