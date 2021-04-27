using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public class ThermostatRepository : BaseRepository, IThermostatRepository
    {
        public ThermostatRepository(HomeAssistantContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Thermostat>> GetThermostatsAsync(string email, Guid houseId, Guid roomId)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await Context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            return await Context.Thermostats.Where(thermostat => thermostat.RoomId == room.Id).ToListAsync();
        }

        public async Task<Thermostat> GetThermostatByIdAsync(string email, Guid houseId, Guid roomId, Guid id)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await Context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            return await Context.Thermostats.Where(thermostat => thermostat.RoomId == room.Id)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Thermostat> CreateThermostatAsync(string email, Guid houseId, Guid roomId,
            Thermostat thermostat)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await Context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            thermostat.RoomId = room.Id;
            Thermostat newThermostat = (await Context.Thermostats.AddAsync(thermostat)).Entity;
            await Context.SaveChangesAsync();
            return newThermostat;
        }

        public async Task<Thermostat> DeleteThermostatAsync(string email, Guid houseId, Guid roomId, Guid id)
        {
            House house = await Context.Houses.Where(h => h.Email == email)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await Context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            Thermostat thermostat = await Context.Thermostats.Where(t => t.RoomId == room.Id)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (thermostat == null)
            {
                return null;
            }

            Context.Thermostats.Remove(thermostat);
            await Context.SaveChangesAsync();
            return thermostat;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync() >= 0;
        }
    }
}