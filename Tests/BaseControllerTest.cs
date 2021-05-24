using System;
using System.Net.Http;
using System.Net.Http.Headers;
using API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared;

namespace Tests
{
    public class BaseControllerTest
    {
        protected string GetApiUrl()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true).Build();
            return configuration.GetValue<string>("ApiUrl");
        }

        protected HttpClient GetClient(string name)
        {
            WebApplicationFactory<Startup> applicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<HomeAssistantContext>));
                        services.AddDbContext<HomeAssistantContext>(options => { options.UseInMemoryDatabase(name); });
                    });
                });
            using (IServiceScope serviceScope = applicationFactory.Services.CreateScope())
            {
                IServiceProvider serviceProvider = serviceScope.ServiceProvider;
                HomeAssistantContext context = serviceProvider.GetRequiredService<HomeAssistantContext>();
                context.Database.EnsureDeleted();
                DatabaseInitializer.Initialize(context);
            }

            HttpClient client = applicationFactory.CreateClient();
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true).Build();
            string token = configuration.GetValue<string>("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}