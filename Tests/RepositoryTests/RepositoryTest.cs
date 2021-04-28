using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Models;

namespace Tests.RepositoryTests
{
    public abstract class RepositoryTest : IDisposable
    {
        protected readonly HomeAssistantContext Context;
        protected readonly IMapper Mapper;

        protected RepositoryTest()
        {
            var options = new DbContextOptionsBuilder<HomeAssistantContext>()
                .UseInMemoryDatabase("HomeAssistantTest").Options;
            Context = new HomeAssistantContext(options);
            Context.Database.EnsureCreated();
            DatabaseInitializer.Initialize(Context);

            MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression =>
            {
                mapperConfigurationExpression.AddProfile(new MappingProfile());
            });
            Mapper = mapperConfiguration.CreateMapper();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}