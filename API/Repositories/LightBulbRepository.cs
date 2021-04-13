using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class LightBulbRepository : ILightBulbRepository
    {
        private readonly HomeAssistantContext _context;

        public LightBulbRepository(HomeAssistantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LightBulb>> GetLightBulbsAsync(int userId, int houseId, int roomId)
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

            return await _context.LightBulbs.Where(lb => lb.RoomId == room.Id).ToListAsync();
        }

        public async Task<LightBulb> GetLightBulbByIdAsync(int userId, int houseId, int roomId, int id)
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

            LightBulb lightBulb = await _context.LightBulbs.Where(lb => lb.RoomId == room.Id)
                .FirstOrDefaultAsync(lb => lb.Id == id);

            return lightBulb;
        }

        public async Task<LightBulb> CreateLightBulb(int userId, int houseId, int roomId, LightBulb lightBulb)
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

            lightBulb.RoomId = room.Id;
            LightBulb newLightBulb = (await _context.LightBulbs.AddAsync(lightBulb)).Entity;
            await _context.SaveChangesAsync();
            return newLightBulb;
        }

        public async Task<LightBulb> DeleteLightBulb(int userId, int houseId, int roomId, int id)
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

            LightBulb lightBulb = await _context.LightBulbs.Where(lb => lb.RoomId == room.Id)
                .FirstOrDefaultAsync(lb => lb.Id == id);
            if (lightBulb == null)
            {
                return null;
            }

            _context.LightBulbs.Remove(lightBulb);
            await _context.SaveChangesAsync();
            return lightBulb;
        }

        public Task<LightBulb> UpdateLightBulb(int userId, int houseId, int roomId, LightBulb lightBulb)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            //_context?.Dispose();
        }
    }
}