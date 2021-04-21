using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Tests
{
    public class Database : IDisposable
    {
        protected readonly HomeAssistantContext Context;

        protected Database()
        {
            var options = new DbContextOptionsBuilder<HomeAssistantContext>().UseInMemoryDatabase("Test")
                .Options;
            Context = new HomeAssistantContext(options);
            Context.Database.EnsureCreated();
            DatabaseInitializer.Initialize(Context);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}