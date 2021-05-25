using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public abstract class BaseRepository
    {
        private readonly Dictionary<string, int> _limits = new()
        {
            {"houses", 1},
            {"rooms", 2},
            {"lightBulbs", 1},
            {"doors", 1},
            {"thermostats", 1},
            {"schedules", 1},
            {"lightBulbsCommands", 1},
            {"doorsCommands", 1},
            {"thermostatsCommands", 1}
        };

        protected readonly HomeAssistantContext Context;
        protected readonly IMapper Mapper;

        protected BaseRepository(HomeAssistantContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        private static string ToUpper(string input)
        {
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        protected static void CheckString(string field, string name)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                throw new ArgumentException($"{ToUpper(name)} cannot be empty.");
            }
        }

        protected static void CheckGuid(Guid field, string name)
        {
            if (field == Guid.Empty)
            {
                throw new ArgumentException($"{ToUpper(name)} cannot be empty.");
            }
        }

        protected async Task<int> GetLimit(string type, string email)
        {
            int limit = _limits[type];

            UserSubscription userSubscription = await Context.UserSubscriptions.FirstOrDefaultAsync(us =>
                us.Email == email);
            if (userSubscription != null)
            {
                Subscription subscription = await Context.Subscriptions.FirstOrDefaultAsync(s => s.Id ==
                    userSubscription.SubscriptionId);
                if (subscription != null)
                {
                    switch (type)
                    {
                        case "houses":
                            limit = subscription.Houses ?? _limits[type];
                            break;
                        case "rooms":
                            limit = subscription.Rooms ?? _limits[type];
                            break;
                        case "lightBulbs":
                            limit = subscription.LightBulbs ?? _limits[type];
                            break;
                        case "doors":
                            limit = subscription.Doors ?? _limits[type];
                            break;
                        case "thermostats":
                            limit = subscription.Thermostats ?? _limits[type];
                            break;
                        case "schedules":
                            limit = subscription.Schedules ?? _limits[type];
                            break;
                        case "lightBulbCommands":
                            limit = subscription.LightBulbCommands ?? _limits[type];
                            break;
                        case "doorCommands":
                            limit = subscription.DoorCommands ?? _limits[type];
                            break;
                        case "thermostatCommands":
                            limit = subscription.ThermostatCommands ?? _limits[type];
                            break;
                        default:
                            limit = 1;
                            break;
                    }
                }
            }

            return limit;
        }

        protected async Task<House> GetHouseInternalAsync(string email, Guid id)
        {
            return await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        protected async Task<Room> GetRoomInternalAsync(string email, Guid houseId, Guid id)
        {
            House house = await GetHouseInternalAsync(email, houseId);
            if (house == null)
            {
                return null;
            }

            return await Context.Rooms.Where(room => room.HouseId == house.Id)
                .FirstOrDefaultAsync(room => room.Id == id);
        }

        protected async Task<Schedule> GetScheduleInternalAsync(string email, Guid id)
        {
            return await Context.Schedules.Where(s => s.Email == email)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}