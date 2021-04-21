using Shared.Models;
using System.Linq;

namespace Tests
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
            House[] houses = {
                new()
                {
                    Email = "gica@popescu.com",
                    Name = "test"
                },
                new()
                {
                    Email = "gicaaa@popescu.com",
                    Name = "test2"
                },
                new()
                {
                    Email = "gicadsadas@popescu.com",
                    Name = "test3"
                },
                new()
                {
                    Email = "marica@dodescu.com",
                    Name = "test0"
                },
                new()
                {
                    Email = "mardsaica@dodescu.com",
                    Name = "test-1"
                },
            };
            context.Houses.AddRange(houses);
            context.SaveChanges();
        }
    }
}