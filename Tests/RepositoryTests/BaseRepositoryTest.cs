using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Tests.RepositoryTests
{
    public abstract class BaseRepositoryTest
    {
        protected HomeAssistantContext GetContextWithData()
        {
            DbContextOptions<HomeAssistantContext> options = new DbContextOptionsBuilder<HomeAssistantContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            HomeAssistantContext context = new HomeAssistantContext(options);
            DatabaseInitializer.Initialize(context);
            return context;
        }

        protected IMapper GetMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression =>
            {
                mapperConfigurationExpression.AddProfile(new MappingProfile());
            });
            return mapperConfiguration.CreateMapper();
        }
    }
}