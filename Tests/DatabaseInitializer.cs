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

            if (!context.Schedules.Any())
            {
                SeedSchedules(context);
            }

            if (!context.LightBulbCommands.Any())
            {
                SeedLightBulbCommands(context);
            }

            if (!context.DoorCommands.Any())
            {
                SeedDoorCommands(context);
            }

            if (!context.ThermostatCommands.Any())
            {
                SeedThermostatCommands(context);
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

        private static void SeedSchedules(HomeAssistantContext context)
        {
            Schedule[] schedules =
            {
                new()
                {
                    Id = Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                    Email = "homeassistantgo@outlook.com",
                    Name = "Night mode",
                    Time = "22:00",
                    Days = 127
                }
            };
            context.Schedules.AddRange(schedules);
            context.SaveChanges();
        }

        private static void SeedLightBulbCommands(HomeAssistantContext context)
        {
            LightBulbCommand[] lightBulbCommands =
            {
                new()
                {
                    Id = Guid.Parse("31048472-de9f-427e-b7af-7a3416928652"),
                    LightBulbId = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"),
                    ScheduleId = Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                    Color = 0,
                    Intensity = 0
                }
            };
            context.LightBulbCommands.AddRange(lightBulbCommands);
            context.SaveChanges();
        }

        private static void SeedDoorCommands(HomeAssistantContext context)
        {
            DoorCommand[] doorCommands =
            {
                new()
                {
                    Id = Guid.Parse("9deee913-0e03-4f5d-a2d3-b459457a570b"),
                    DoorId = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"),
                    ScheduleId = Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                    Locked = true
                }
            };
            context.DoorCommands.AddRange(doorCommands);
            context.SaveChanges();
        }

        private static void SeedThermostatCommands(HomeAssistantContext context)
        {
            ThermostatCommand[] thermostatCommands =
            {
                new()
                {
                    Id = Guid.Parse("e0e835fc-bd16-4698-b2af-00b960df7e04"),
                    ThermostatId = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"),
                    ScheduleId = Guid.Parse("377a7b7b-2b63-4317-bff6-e52ef5eb51da"),
                    Temperature = (decimal) 22.5
                }
            };
            context.ThermostatCommands.AddRange(thermostatCommands);
            context.SaveChanges();
        }
    }
}