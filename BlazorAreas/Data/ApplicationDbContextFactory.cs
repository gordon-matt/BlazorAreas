using Extenso.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlazorAreas.Data
{
    public class ApplicationDbContextFactory : IDbContextFactory
    {
        private readonly IConfiguration configuration;

        public ApplicationDbContextFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private DbContextOptions<ApplicationDbContext> options;

        private DbContextOptions<ApplicationDbContext> Options
        {
            get
            {
                if (options == null)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    optionsBuilder.UseInMemoryDatabase(configuration.GetConnectionString("DefaultConnection"));
                    options = optionsBuilder.Options;
                }
                return options;
            }
        }

        public DbContext GetContext()
        {
            return new ApplicationDbContext(Options);
        }

        public DbContext GetContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(connectionString); // In this case, "connectionString" is just a database name for the in-memory provider
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}