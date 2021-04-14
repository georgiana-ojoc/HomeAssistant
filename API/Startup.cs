using System.Reflection;
using API.Interfaces;
using API.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Shared.Models;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionService.Connection = Configuration["ConnectionString"];
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });

            services.AddHttpContextAccessor();
            services.AddScoped(sp =>
            {
                HttpContext context = sp.GetService<IHttpContextAccessor>()?.HttpContext;
                Identity identity = new Identity();
                if (context?.User.Identity != null && context.User.Identity.IsAuthenticated)
                {
                    identity.Email = context.User.FindFirst("emails")?.Value;
                }

                return identity;
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddControllers();
            services.AddSingleton<HomeAssistantContext>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IHouseRepository, HouseRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IDoorRepository, DoorRepository>();
            services.AddScoped<ILightBulbRepository, LightBulbRepository>();
            services.AddScoped<IThermostatRepository, ThermostatRepository>();

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
                        "Example: \"Bearer 12345jwt\""
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints => { endpoints.MapControllers().RequireAuthorization(); });
        }
    }
}