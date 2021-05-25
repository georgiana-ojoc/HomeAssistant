using System;
using System.Linq;
using System.Threading.Tasks;
using API;
using API.Models;

namespace Tests
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(HomeAssistantContext context)
        {
            if (!context.Subscriptions.Any()) {
                await SeedSubscriptionsAsync(context);
            }
            if (!context.UserSubscriptions.Any()) {
                await SeedUserSubscriptionsAsync(context);
            }
            if (!context.Houses.Any())
            {
                await SeedHousesAsync(context);
            }

            if (!context.Rooms.Any())
            {
                await SeedRoomsAsync(context);
            }

            if (!context.LightBulbs.Any())
            {
                await SeedLightBulbsAsync(context);
            }

            if (!context.Doors.Any())
            {
                await SeedDoorsAsync(context);
            }

            if (!context.Thermostats.Any())
            {
                await SeedThermostatsAsync(context);
            }

            if (!context.Schedules.Any())
            {
                await SeedSchedulesAsync(context);
            }

            if (!context.LightBulbCommands.Any())
            {
                await SeedLightBulbCommandsAsync(context);
            }

            if (!context.DoorCommands.Any())
            {
                await SeedDoorCommandsAsync(context);
            }

            if (!context.ThermostatCommands.Any())
            {
                await SeedThermostatCommandsAsync(context);
            }
        }
        
        private static async Task SeedSubscriptionsAsync(HomeAssistantContext context)
        {
            Subscription[] subscriptions =
            {
                new()
                {
                    Id = Guid.Parse("dc252c6b-6f7d-4e5a-981a-0533b6b57167"),
                    Name = "Basic subscription",
                    Description = "Suitable for small families",
                    Price = 1000,
                    Houses = 2,
                    Rooms = 2,
                    LightBulbs = 3,
                    Doors = 3,
                    Thermostats = 3,
                    Schedules = 2,
                    LightBulbCommands = 2,
                    DoorCommands = 2,
                    ThermostatCommands = 2
                }
            };
            await context.Subscriptions.AddRangeAsync(subscriptions);
            await context.SaveChangesAsync();
        }
        
        private static async Task SeedUserSubscriptionsAsync(HomeAssistantContext context)
        {
            UserSubscription[] userSubscription =
            {
                new()
                {
                    Email = "homeassistantgo@outlook.com",
                    SubscriptionId = Guid.Parse("dc252c6b-6f7d-4e5a-981a-0533b6b57167")
                }
            };
            await context.UserSubscriptions.AddRangeAsync(userSubscription);
            await context.SaveChangesAsync();
        }

        private static async Task SeedHousesAsync(HomeAssistantContext context)
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
            await context.Houses.AddRangeAsync(houses);
            await context.SaveChangesAsync();
        }

        private static async Task SeedRoomsAsync(HomeAssistantContext context)
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
            await context.Rooms.AddRangeAsync(rooms);
            await context.SaveChangesAsync();
        }

        private static async Task SeedLightBulbsAsync(HomeAssistantContext context)
        {
            LightBulb[] lightBulbs =
            {
                new()
                {
                    Id = Guid.Parse("cb57603b-5140-451b-9138-906355464d7a"),
                    RoomId = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                    Name = "Lamp"
                },
                new()
                {
                    Id = Guid.Parse("0365f802-bb3a-487a-997b-0d34c270a385"),
                    RoomId = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                    Name = "Spotlight"
                }
            };
            await context.LightBulbs.AddRangeAsync(lightBulbs);
            await context.SaveChangesAsync();
        }

        private static async Task SeedDoorsAsync(HomeAssistantContext context)
        {
            Door[] doors =
            {
                new()
                {
                    Id = Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"),
                    RoomId = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                    Name = "Balcony door"
                },
                new()
                {
                    Id = Guid.Parse("3968c3e5-daee-4096-a6d4-11b640216591"),
                    RoomId = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                    Name = "Terrace door"
                }
            };
            await context.Doors.AddRangeAsync(doors);
            await context.SaveChangesAsync();
        }

        private static async Task SeedThermostatsAsync(HomeAssistantContext context)
        {
            Thermostat[] thermostats =
            {
                new()
                {
                    Id = Guid.Parse("ec7c38a2-c391-4294-b436-dd5c0d71494e"),
                    RoomId = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                    Name = "Wall thermostat"
                },
                new()
                {
                    Id = Guid.Parse("c207eada-509b-4655-9f99-d3be6786e895"),
                    RoomId = Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                    Name = "Thermostat"
                }
            };
            await context.Thermostats.AddRangeAsync(thermostats);
            await context.SaveChangesAsync();
        }

        private static async Task SeedSchedulesAsync(HomeAssistantContext context)
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
            await context.Schedules.AddRangeAsync(schedules);
            await context.SaveChangesAsync();
        }

        private static async Task SeedLightBulbCommandsAsync(HomeAssistantContext context)
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
            await context.LightBulbCommands.AddRangeAsync(lightBulbCommands);
            await context.SaveChangesAsync();
        }

        private static async Task SeedDoorCommandsAsync(HomeAssistantContext context)
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
            await context.DoorCommands.AddRangeAsync(doorCommands);
            await context.SaveChangesAsync();
        }

        private static async Task SeedThermostatCommandsAsync(HomeAssistantContext context)
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
            await context.ThermostatCommands.AddRangeAsync(thermostatCommands);
            await context.SaveChangesAsync();
        }
    }
}