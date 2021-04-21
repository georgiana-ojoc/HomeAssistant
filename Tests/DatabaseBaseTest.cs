using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Tests
{
    public class DatabaseBaseTest
    {
        protected readonly HomeAssistantContext context;

        public DatabaseBaseTest()
        {
            var options = new DbContextOptionsBuilder<HomeAssistantContext>().UseInMemoryDatabase("Test").Options;
            context = new HomeAssistantContext(options);
            context.Database.EnsureCreated();
            DatabaseInitializer.Initialize(context);
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}