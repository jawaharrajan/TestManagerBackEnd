using TestManager.DataAccess;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Manager_Back_End_Tests
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext Create(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted(); // ensure clean slate
            context.Database.EnsureCreated();

            return context;
        }
    }
}
