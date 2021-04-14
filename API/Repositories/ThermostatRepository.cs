using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Repositories
{
    public class ThermostatRepository : IThermostatRepository
    {
        private readonly HomeAssistantContext _context;

        public ThermostatRepository(HomeAssistantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Thermostat>> GetThermostatsAsync(int userId, int houseId, int roomId)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            return await _context.Thermostats.Where(t => t.RoomId == room.Id).ToListAsync();
        }

        public async Task<Thermostat> GetThermostatByIdAsync(int userId, int houseId, int roomId, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            Thermostat thermostat = await _context.Thermostats.Where(t => t.RoomId == room.Id)
                .FirstOrDefaultAsync(d => d.Id == id);

            return thermostat;
        }

        public async Task<Thermostat> CreateThermostatAsync(int userId, int houseId, int roomId, Thermostat thermostat)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            thermostat.RoomId = room.Id;
            Thermostat newThermostat = (await _context.Thermostats.AddAsync(thermostat)).Entity;
            await _context.SaveChangesAsync();
            return newThermostat;
        }

        public async Task<Thermostat> DeleteThermostatAsync(int userId, int houseId, int roomId, int id)
        {
            House house = await _context.Houses.Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(h => h.Id == houseId);
            if (house == null)
            {
                return null;
            }

            Room room = await _context.Rooms.Where(r => r.HouseId == house.Id)
                .FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                return null;
            }

            Thermostat thermostat = await _context.Thermostats.Where(t => t.RoomId == room.Id)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (thermostat == null)
            {
                return null;
            }

            _context.Thermostats.Remove(thermostat);
            await _context.SaveChangesAsync();
            return thermostat;
        }

        public Task<Thermostat> UpdateThermostatAsync(int userId, int houseId, int roomId, Thermostat thermostat)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            //_context.Dispose();
        }
    }
}