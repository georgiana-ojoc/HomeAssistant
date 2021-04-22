using System.Linq;
using Shared.Models;

namespace Tests.RepositoryTests
{
    public static class DatabaseInitializer
    {
        public static void Initialize(HomeAssistantContext context)
        {
            if (!context.Houses.Any())
            {
                SeedHouses(context);
            }
        }

        private static void SeedHouses(HomeAssistantContext context)
        {
            House[] houses =
            {
                new()
                {
                    Email = "gica@popescu.com",
                    Name = "Apartment"
                },
                new()
                {
                    Email = "costel@popescu.com",
                    Name = "Apartment"
                },
                new()
                {
                    Email = "ion@popescu.com",
                    Name = "Apartment"
                },
                new()
                {
                    Email = "ilie@dodescu.com",
                    Name = "Apartment"
                },
                new()
                {
                    Email = "marius@dodescu.com",
                    Name = "Apartment"
                },
            };
            context.Houses.AddRange(houses);
            context.SaveChanges();
        }
    }
}