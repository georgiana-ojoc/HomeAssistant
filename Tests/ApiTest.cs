using System.Net.Http;
using System.Net.Http.Headers;
using API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Models;

namespace Tests
{
    public class ApiTest
    {
        protected readonly string ApiUrl;
        protected readonly HttpClient Client;

        protected ApiTest()
        {
            var configuration = new ConfigurationBuilder().AddUserSecrets<ApiTest>().Build();

            ApiUrl = configuration.GetValue<string>("ApiUrl");

            WebApplicationFactory<Startup> applicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<HomeAssistantContext>));
                        services.AddDbContext<HomeAssistantContext>(options =>
                        {
                            options.UseInMemoryDatabase("HomeAssistantTests");
                        });
                    });
                });

            Client = applicationFactory.CreateClient();

            string token = configuration.GetValue<string>("Token");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}