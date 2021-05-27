using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Client.Utility;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

namespace Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:ApiKey"];

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("API"));

            builder.Services
                .AddHttpClient("API",
                    client =>
                    {
                        client.BaseAddress =
                            new Uri(builder.Configuration["ApiUrl"] ?? string.Empty);
                    })
                .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                    .ConfigureHandler(new[]
                        {
                            builder.Configuration["ApiUrl"]
                        },
                        new[]
                        {
                            builder.Configuration["AzureAdB2C:Scope"]
                        }));

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration["AzureAdB2C:Scope"]);
            });

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<IdService>();

            await builder.Build().RunAsync();
        }
    }
}