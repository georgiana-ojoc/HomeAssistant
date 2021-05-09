using System;
using System.Linq;
using Shared;
using Shared.Models;

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

            if (!context.Rooms.Any())
            {
                SeedRooms(context);
            }

            if (!context.LightBulbs.Any())
            {
                SeedLightBulbs(context);
            }

            if (!context.Doors.Any())
            {
                SeedDoors(context);
            }

            if (!context.Thermostats.Any())
            {
                SeedThermostats(context);
            }
        }

        private static void SeedHouses(HomeAssistantContext context)
        {
            House[] houses =
            {
                new()
                {
                    Id = Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                    Email = "homeassistantgo@outlook.com",
                    Name = "Apartment"
                }
            };
            context.Houses.AddRange(houses);
            context.SaveChanges();
        }

        private static void SeedRooms(HomeAssistantContext context)
        {
            Room[] rooms =
            {
                new()
                {
                    Id = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                    HouseId = Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                    Name = "Kitchen"
                }
            };
            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }

        private static void SeedLightBulbs(HomeAssistantContext context)
        {
            LightBulb[] lightBulbs =
            {
                new()
                {
                    Id = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"),
                    RoomId = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                    Name = "Lamp"
                }
            };
            context.LightBulbs.AddRange(lightBulbs);
            context.SaveChanges();
        }

        private static void SeedDoors(HomeAssistantContext context)
        {
            Door[] doors =
            {
                new()
                {
                    Id = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"),
                    RoomId = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                    Name = "Balcony door"
                }
            };
            context.Doors.AddRange(doors);
            context.SaveChanges();
        }

        private static void SeedThermostats(HomeAssistantContext context)
        {
            Thermostat[] thermostats =
            {
                new()
                {
                    Id = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"),
                    RoomId = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                    Name = "Wall thermostat"
                }
            };
            context.Thermostats.AddRange(thermostats);
            context.SaveChanges();
        }
    }
}