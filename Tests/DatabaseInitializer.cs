using System;
using Shared.Models;

namespace Tests
{
    public class DatabaseInitializer
    {
        public static void Initialize(HomeAssistantContext context)
        {
            // TODO if(context not empty do something)

            Seed(context);
        }

        private static void Seed(HomeAssistantContext context)
        {
            var houses = new[]
            {
                new House
                {
                    Email = "gica@popescu.com",
                    Name = "test"
                },
                new House
                {
                    Email = "gicaaa@popescu.com",
                    Name = "test2"
                },
                new House
                {
                    Email = "gicadsadas@popescu.com",
                    Name = "test3"
                },
                new House
                {
                    Email = "marica@dodescu.com",
                    Name = "test0"
                },
                new House
                {
                    Email = "mardsaica@dodescu.com",
                    Name = "test-1"
                },
            };
            context.AddRange(houses);
            context.SaveChanges();
        }
    }
}