using System.Reflection;
using API.Interfaces;
using API.Repositories;
using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Stripe;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionService.Connection = Configuration["ConnectionString"];
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["ApiKey"];
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5000", "https://localhost:5001",
                                "http://localhost:6000", "https://localhost:6001")
                            .AllowAnyMethod().AllowAnyHeader();
                    });
            });

            services.AddHttpContextAccessor();
            services.AddScoped(serviceProvider =>
            {
                HttpContext context = serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
                Identity identity = new Identity();
                if (context?.User.Identity != null && context.User.Identity.IsAuthenticated)
                {
                    identity.Email = context.User.FindFirst("emails")?.Value;
                }

                return identity;
            });

            services.AddControllers().AddNewtonsoftJson();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddDbContext<HomeAssistantContext>(options =>
                options.UseSqlServer(ConnectionService.Connection,
                    builder => builder.EnableRetryOnFailure()));

            MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression =>
            {
                mapperConfigurationExpression.AddProfile(new MappingProfile());
            });

            services.AddSingleton(mapperConfiguration.CreateMapper());

            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IUserSubscriptionRepository, UserSubscriptionRepository>();
            services.AddScoped<IHouseRepository, HouseRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<ILightBulbRepository, LightBulbRepository>();
            services.AddScoped<IDoorRepository, DoorRepository>();
            services.AddScoped<IThermostatRepository, ThermostatRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ILightBulbCommandRepository, LightBulbCommandRepository>();
            services.AddScoped<IDoorCommandRepository, DoorCommandRepository>();
            services.AddScoped<IThermostatCommandRepository, ThermostatCommandRepository>();

            services.AddHangfire(configuration => configuration.UseSqlServerStorage(ConnectionService.Connection));
            services.AddHangfireServer();

            services.AddScoped<Helper>();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Home Assistant API",
                    Description = "ASP.NET Core 5.0 Web API"
                });
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme.\n\n" +
                        "Enter \"Bearer\" [space] and then your token in the text input below.\n\n" +
                        "Example: \"Bearer 12345token\""
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        System.Array.Empty<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(swagger => swagger.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Home Assistant API v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints => { endpoints.MapControllers().RequireAuthorization(); });

            app.UseHangfireDashboard();
        }
    }
}